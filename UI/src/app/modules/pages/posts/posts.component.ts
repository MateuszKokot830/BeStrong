import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Post } from 'src/app/core/models/Post';
import { User, UserAuth } from 'src/app/core/models/User';
import { AccountService } from 'src/app/core/services/account.service';
import { PostService } from 'src/app/core/services/post.service';
import { UserService } from 'src/app/core/services/user.service';
import { AddCommentComponent } from '../../components/add-comment/add-comment.component';
import { AddPostComponent } from '../../components/add-post/add-post.component';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {
  currentUser: UserAuth | null = null;
  user: User;
  followedUsers: User[] = [];
  followerPosts: Post[] = [];
  newPost = {} as Post;
  newComment = {} as Comment;
  bsModalRef: BsModalRef;

  constructor(private accountService: AccountService, private userService: UserService,
    private toastr: ToastrService, private postService: PostService, private modalService: BsModalService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: currentUser => this.currentUser = currentUser
      });
    }

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    this.userService.getUser(this.currentUser.username).subscribe({
      next: user => {
        this.user = user;
        var followedUsersIds = [];
        this.user.followedUsers.forEach(x => {
          followedUsersIds.push(x.followedUserId);
        });
        this.userService.getFollowerUsers(followedUsersIds).subscribe({
          next: users => {
            this.followedUsers = users;
          }
        })
        followedUsersIds.push(this.user.id);
        this.postService.getFollowerPosts(followedUsersIds).subscribe({
          next: posts => {
            this.followerPosts = posts;
          }
        })
      }
    });
  }

  getUsername(id: number) {
    if (this.user.id == id) {
      return this.user.userName;
    }
    else
    {
      return this.followedUsers.find(x => x.id == id).userName;
    }
  }

  getMainPhoto(id: number) {
    if (this.user.id == id) {
      return this.user.profilePhotoUrl;
    }
    else
    {
      return this.followedUsers.find(x => x.id == id).profilePhotoUrl;
    }
  }

  addNewPostToggle() {
    const initialState = {
      user: this.user
    };
    this.bsModalRef = this.modalService.show(AddPostComponent, {initialState});
  }

  addNewCommentToggle(post: Post) {
    const initialState = {
      user: this.user,
      post: post
    };
    this.bsModalRef = this.modalService.show(AddCommentComponent, {initialState});
  }

}
