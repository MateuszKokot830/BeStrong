import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Exercise } from 'src/app/core/models/Exercise';
import { Photo } from 'src/app/core/models/Photo';
import { User, UserAuth } from 'src/app/core/models/User';
import { Workout } from 'src/app/core/models/Workout';
import { AccountService } from 'src/app/core/services/account.service';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User | undefined;
  currentUser: UserAuth | null = null;
  currentUserAcc: User | null = null;
  workouts: Workout[] = [];
  exercises: Exercise[] = [];
  lastWorkout: Workout;
  sinceLastWorkout: string;
  followers: User[] = [];
  followedUsers: User[] = [];
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  isCurrentUser: boolean;
  isEditMode: boolean;
  isFollowed: boolean;
  fileName = "";
  currentPhoto: Photo | null = null;

  constructor(private userService: UserService, private accountService: AccountService,
    private route: ActivatedRoute, private toastr: ToastrService, private workoutService: WorkoutService,
    private http: HttpClient) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: currentUser => this.currentUser = currentUser
    });
  }

  ngOnInit(): void {
    this.loadProfile();
    this.loadExercises();
  }

  loadProfile() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;

    this.userService.getUser(username).subscribe({
      next: user => {
        this.user = user;
        this.galleryImages = this.loadGallery();
        this.loadWorkouts();
        if (this.user.userName == this.currentUser.username)
        {
          this.isCurrentUser = true;
        }
        else
        {
          this.userService.getUser(this.currentUser.username).subscribe({
            next: userAcc => this.currentUserAcc = userAcc
          });
        }
        this.loadFollowedUsers();
        this.loadFollowers();
      }
    });
  }

  loadGallery() {
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]

    const imageUrls = [];
    for (const photo of this.user.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      });
    }

    if (this.user.photos.length > 0) this.currentPhoto = this.user.photos[0];
    return imageUrls;
  }

  onChange(data: any): void {
    this.currentPhoto = this.user.photos[data.index];
  }

  changeEditMode() {
    this.isEditMode = !this.isEditMode;
  }

  saveChanges() {
    this.userService.updateUser(this.user).subscribe({
      next: _ => {
        this.changeEditMode();
        this.toastr.success('Profile has been updated');
      }
    });
  }

  loadFollowedUsers() {
    var followedUsersIds = [];
    this.user.followedUsers.forEach(x => {
      followedUsersIds.push(x.followedUserId);
    });
    this.userService.getFollowerUsers(followedUsersIds).subscribe({
      next: users => {
        this.followedUsers = users;
      }
    })
  }

  loadFollowers() {
    var followersIds = [];
    this.user.followers.forEach(x => {
      followersIds.push(x.userId);
    });
    this.userService.getFollowerUsers(followersIds).subscribe({
      next: users => {
        this.followers = users;
        if (followersIds.includes(this.currentUserAcc.id)) this.isFollowed = true;
      }
    })
  }

  loadWorkouts() {
    this.workoutService.getUserWorkouts(this.user).subscribe({
      next: workouts => {
        this.workouts = workouts;
        if (this.workouts.length != 0) {
          this.lastWorkout = this.workouts[0];
          var dateDifference = new Date().getTime() - new Date(this.lastWorkout.date).getTime();
          this.sinceLastWorkout = Math.round(dateDifference / (1000 * 3600 * 24)) + ' days ago';
        }
      }
    });
  }

  loadExercises() {
    this.workoutService.getExercises().subscribe({
      next: exercises => this.exercises = exercises
    })
  }

  getExerciseName(id: number) {
    return this.exercises.find(x => x.id == id).name;
  }

  followUser(id: number) {
    return this.userService.followUser(id).subscribe();
  }

  onFileSelected(event) {
    const file:File = event.target.files[0];

    if (file) {
        this.fileName = file.name;
        const formData = new FormData();
        formData.append('file', file);
        return this.userService.addPhoto(formData, this.user.id).subscribe({
          next: _ => {
            this.toastr.success('Photo has been added');
          }
        });
    }
  }

  setMainPhoto() {
    return this.userService.setMainPhoto(this.currentPhoto.id, this.user.id).subscribe({
      next: _ => {
        this.toastr.success('Main photo has been changed');
      }
    });
  }

  deletePhoto() {
    return this.userService.deletePhoto(this.currentPhoto.id, this.user.id).subscribe({
      next: _ => {
        this.toastr.success('Photo has been deleted');
      }
    });
  }
}
