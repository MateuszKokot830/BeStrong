import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { User, UserAuth } from 'src/app/core/models/User';
import { AccountService } from 'src/app/core/services/account.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User | undefined;
  currentUser: UserAuth | null = null;
  followers: User[] = [];
  follows: User[] = [];
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  isCurrentUser: boolean;
  isEditMode: boolean;

  constructor(private userService: UserService, private accountService: AccountService,
    private route: ActivatedRoute, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: currentUser => this.currentUser = currentUser
    });
  }

  ngOnInit(): void {
    this.loadProfile();
    this.loadUsers();
  }

  loadProfile() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;

    this.userService.getUser(username).subscribe({
      next: user => {
        this.user = user;
        this.galleryImages = this.loadGallery();
        if (this.user.userName == this.currentUser.username) this.isCurrentUser = true;
      }
    });

    console.log(this.isCurrentUser);
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
      })
    }
    return imageUrls;
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

  loadUsers() {
    this.userService.getUsers().subscribe({
      next: users => {
        this.followers = users.slice(-4),
        this.follows = users.slice(-3)
      }
    })
  }
}
