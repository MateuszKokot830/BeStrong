import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Photo } from 'src/app/core/models/Photo';
import { Measurements, User, UserUpdate } from 'src/app/core/models/User';
import { Workout, WorkoutExercise } from 'src/app/core/models/Workout';
import { AccountService } from 'src/app/core/services/account.service';
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
  isCurrentUser = false;
  isEditMode = false;
  isFollowed = false;
  isLoading = false;
  isSaving = false;
  currentPhoto: Photo | null = null;

  constructor(private accountService: AccountService, private userService: UserService,
    private route: ActivatedRoute, private toastr: ToastrService,
    private workoutService: WorkoutService) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username)
      return;

    this.isLoading = true;

    forkJoin({
      user: this.userService.getUser(username),
      currentUserAcc: this.accountService.currentProfile(),
      exercises: this.workoutService.getExercises()
    }).pipe(
      switchMap(({ user, currentUserAcc, exercises }) => {
        this.applyUser(user, currentUserAcc);
        this.exerciseNames = new Map(exercises.map(e => [e.id, e.name ?? 'Unknown exercise']));

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
        this.isLoading = false;
        this.followers = followers;
        this.followedUsers = followedUsers;
        this.setWorkouts(workouts);
      },
      error: _ => this.isLoading = false
    });
  }

  private applyUser(user: User, currentUserAcc: User) {
    this.user = user;
    this.currentUserAcc = currentUserAcc;
    this.isCurrentUser = user.id === currentUserAcc.id;
    this.isFollowed = user.followers.some(f => f.userId === currentUserAcc.id);
    this.measurements = user.measurements ?? emptyMeasurements();
    this.currentPhoto = user.photos.length > 0 ? user.photos[0] : null;
  }

  private reloadUser() {
    const user = this.user;
    const currentUserAcc = this.currentUserAcc;
    if (!user || !currentUserAcc)
      return;

    const request = this.isCurrentUser
      ? this.accountService.refreshProfile().pipe(switchMap(me => forkJoin({
          user: this.userService.getUser(user.userName),
          me: of(me)
        })))
      : forkJoin({
          user: this.userService.getUser(user.userName),
          me: of(currentUserAcc)
        });

    request.subscribe({
      next: ({ user: fresh, me }) => this.applyUser(fresh, me)
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

  onPhotoChange(photo: Photo): void {
    this.currentPhoto = photo;
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

    this.isSaving = true;
    this.userService.updateUser(update).subscribe({
      next: updated => {
        this.isSaving = false;
        this.user = updated;
        this.measurements = updated.measurements ?? emptyMeasurements();
        this.accountService.invalidateProfile();
        this.changeEditMode();
        this.toastr.success('Profile has been updated');
      },
      error: _ => this.isSaving = false
    });
  }

  getExerciseName(exerciseId: number) {
    return this.exerciseNames.get(exerciseId) ?? 'Unknown exercise';
  }

  formatReps(exercise: WorkoutExercise) {
    const reps = exercise.sets.map(s => s.reps);
    if (!reps.length)
      return '-';

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

    const wasFollowed = this.isFollowed;
    const request = wasFollowed
      ? this.userService.unfollowUser(this.currentUserAcc.id, this.user.id)
      : this.userService.followUser(this.currentUserAcc.id, this.user.id);

    request.subscribe({
      next: _ => {
        this.accountService.invalidateProfile();
        this.toastr.success(wasFollowed ? 'Unfollowed' : 'Now following');
        this.loadProfile();
      }
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
        this.toastr.success('Photo has been added');
        this.reloadUser();
      },
      complete: () => input.value = ''
    });
  }

  setMainPhoto() {
    if (!this.currentPhoto || !this.user)
      return;

    this.userService.setMainPhoto(this.currentPhoto.id, this.user.id).subscribe({
      next: _ => {
        this.toastr.success('Main photo has been changed');
        this.reloadUser();
      }
    });
  }

  deletePhoto() {
    if (!this.currentPhoto || !this.user)
      return;

    this.userService.deletePhoto(this.currentPhoto.id, this.user.id).subscribe({
      next: _ => {
        this.toastr.success('Photo has been deleted');
        this.reloadUser();
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
