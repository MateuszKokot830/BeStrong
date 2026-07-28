import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Photo } from 'src/app/core/models/Photo';
import { Measurements, User, UserUpdate } from 'src/app/core/models/User';
import { Workout, WorkoutExercise } from 'src/app/core/models/Workout';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User | null = null;
  currentUserAcc: User | null = null;
  measurements: Measurements = emptyMeasurements();
  workouts: Workout[] = [];
  exerciseNames = new Map<number, string>();
  lastWorkout: Workout | null = null;
  sinceLastWorkout = '';
  followers: User[] = [];
  followedUsers: User[] = [];
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  isCurrentUser = false;
  isEditMode = false;
  isFollowed = false;
  currentPhoto: Photo | null = null;

  constructor(private userService: UserService, private route: ActivatedRoute,
    private toastr: ToastrService, private workoutService: WorkoutService) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username)
      return;

    forkJoin({
      user: this.userService.getUser(username),
      currentUserAcc: this.userService.getCurrentUser(),
      exercises: this.workoutService.getExercises()
    }).pipe(
      switchMap(({ user, currentUserAcc, exercises }) => {
        this.user = user;
        this.currentUserAcc = currentUserAcc;
        this.isCurrentUser = user.id === currentUserAcc.id;
        this.isFollowed = user.followers.some(f => f.userId === currentUserAcc.id);
        this.exerciseNames = new Map(exercises.map(e => [e.id, e.name ?? 'Unknown exercise']));
        this.measurements = user.measurements ?? emptyMeasurements();
        this.galleryImages = this.loadGallery(user);

        const followerIds = user.followers.map(f => f.userId);
        const followedIds = user.followedUsers.map(f => f.followedUserId);

        return forkJoin({
          followers: followerIds.length ? this.userService.getUsersByIds(followerIds) : of<User[]>([]),
          followedUsers: followedIds.length ? this.userService.getUsersByIds(followedIds) : of<User[]>([]),
          workouts: this.workoutService.getUserWorkouts(user.id)
        });
      })
    ).subscribe({
      next: ({ followers, followedUsers, workouts }) => {
        this.followers = followers;
        this.followedUsers = followedUsers;
        this.setWorkouts(workouts);
      }
    });
  }

  private setWorkouts(workouts: Workout[]) {
    this.workouts = workouts;
    if (!workouts.length)
      return;

    this.lastWorkout = workouts[0];
    const dateDifference = new Date().getTime() - new Date(this.lastWorkout.date).getTime();
    this.sinceLastWorkout = Math.round(dateDifference / (1000 * 3600 * 24)) + ' days ago';
  }

  loadGallery(user: User): NgxGalleryImage[] {
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    const imageUrls = user.photos.map(photo => ({
      small: photo.url ?? undefined,
      medium: photo.url ?? undefined,
      big: photo.url ?? undefined
    }));

    if (user.photos.length > 0) this.currentPhoto = user.photos[0];
    return imageUrls;
  }

  onChange(data: { index: number }): void {
    if (!this.user)
      return;

    this.currentPhoto = this.user.photos[data.index];
  }

  changeEditMode() {
    this.isEditMode = !this.isEditMode;
  }

  saveChanges() {
    const user = this.user;
    if (!user)
      return;

    const update: UserUpdate = {
      id: user.id,
      dateOfBirth: user.dateOfBirth,
      dateOfWorkoutStart: user.dateOfWorkoutStart,
      name: user.name,
      surname: user.surname,
      gender: user.gender,
      city: user.city,
      country: user.country,
      description: user.description,
      measurements: this.measurements,
      photos: user.photos
    };

    this.userService.updateUser(update).subscribe({
      next: updated => {
        this.user = updated;
        this.measurements = updated.measurements ?? emptyMeasurements();
        this.changeEditMode();
        this.toastr.success('Profile has been updated');
      }
    });
  }

  getExerciseName(exerciseId: number) {
    return this.exerciseNames.get(exerciseId) ?? 'Unknown exercise';
  }

  formatReps(exercise: WorkoutExercise) {
    const reps = exercise.sets.map(s => s.reps);
    if (!reps.length) return '-';

    return reps.every(r => r === reps[0])
      ? `${reps.length}x${reps[0]}`
      : reps.join(' / ');
  }

  formatWeight(exercise: WorkoutExercise) {
    const weights = exercise.sets.map(s => s.weight ?? 0);
    if (!weights.length)
      return '-';

    return weights.every(w => w === weights[0])
      ? `${weights[0]} kg`
      : `${weights.join(' / ')} kg`;
  }

  toggleFollow() {
    if (!this.user || !this.currentUserAcc)
      return;

    const request = this.isFollowed
      ? this.userService.unfollowUser(this.currentUserAcc.id, this.user.id)
      : this.userService.followUser(this.currentUserAcc.id, this.user.id);

    request.subscribe({
      next: _ => location.reload()
    });
  }

  onFileSelected(event: Event) {
    if (!this.user)
      return;

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file)
      return;

    const formData = new FormData();
    formData.append('file', file);
    this.userService.addPhoto(formData, this.user.id).subscribe({
      next: _ => {
        location.reload();
        this.toastr.success('Photo has been added');
      }
    });
  }

  setMainPhoto() {
    if (!this.currentPhoto || !this.user)
      return;

    this.userService.setMainPhoto(this.currentPhoto.id, this.user.id).subscribe({
      next: _ => {
        location.reload();
        this.toastr.success('Main photo has been changed');
      }
    });
  }

  deletePhoto() {
    if (!this.currentPhoto || !this.user)
      return;

    this.userService.deletePhoto(this.currentPhoto.id, this.user.id).subscribe({
      next: _ => {
        location.reload();
        this.toastr.success('Photo has been deleted');
      }
    });
  }
}

function emptyMeasurements(): Measurements {
  return {
    height: null,
    weight: null,
    chest: null,
    shoulders: null,
    arms: null,
    waist: null,
    hips: null,
    thights: null
  };
}
