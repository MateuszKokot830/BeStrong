import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/core/models/User';
import { AccountService } from 'src/app/core/services/account.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
    selector: 'app-user-card',
    templateUrl: './user-card.component.html',
    styleUrls: ['./user-card.component.css'],
    standalone: false
})
export class UserCardComponent implements OnInit {
  @Input() user!: User;

  currentUserId: number | null = null;
  isFollowed = false;

  constructor(private router: Router, private accountService: AccountService,
    private userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.accountService.currentProfile().subscribe(me => {
      this.currentUserId = me.id;
      this.isFollowed = this.user.followers.some(f => f.userId === me.id);
    });
  }

  get isOwnCard(): boolean {
    return this.currentUserId === this.user.id;
  }

  goToProfile(username: string) {
    this.router.navigate(['/profile/' + username]);
  }

  toggleFollow(event: Event) {
    event.stopPropagation();
    const currentUserId = this.currentUserId;
    if (currentUserId === null)
      return;

    if (this.isFollowed) {
      if (!confirm(`Unfollow ${this.user.userName}?`))
        return;

      this.userService.unfollowUser(currentUserId, this.user.id).subscribe({
        next: _ => {
          this.isFollowed = false;
          this.toastr.success(`Unfollowed ${this.user.userName}`);
        }
      });
    } else {
      this.userService.followUser(currentUserId, this.user.id).subscribe({
        next: _ => {
          this.isFollowed = true;
          this.toastr.success(`Now following ${this.user.userName}`);
        }
      });
    }
  }
}
