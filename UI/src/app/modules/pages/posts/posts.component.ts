import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Post } from 'src/app/core/models/Post';
import { User } from 'src/app/core/models/User';
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
  user: User | null = null;
  posts: Post[] = [];
  authors = new Map<number, User>();
  isLoading = false;

  constructor(private accountService: AccountService, private userService: UserService,
    private postService: PostService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadFeed();
  }

  loadFeed() {
    this.isLoading = true;

    this.accountService.currentProfile().pipe(
      switchMap(user => {
        const followedIds = user.followedUsers.map(f => f.followedUserId);

        return forkJoin({
          me: of(user),
          followedUsers: followedIds.length ? this.userService.getUsersByIds(followedIds) : of<User[]>([]),
          feed: this.postService.getFeed(),
          ownPosts: this.userService.getUserPosts(user.userName)
        });
      })
    ).subscribe({
      next: ({ me, followedUsers, feed, ownPosts }) => {
        this.isLoading = false;
        this.user = me;
        this.authors = new Map(followedUsers.map(u => [u.id, u]));
        this.authors.set(me.id, me);

        this.posts = [...feed, ...ownPosts].sort(
          (a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
        );
      },
      error: _ => this.isLoading = false
    });
  }

  getUsername(userId: number) {
    return this.authors.get(userId)?.userName ?? 'Unknown user';
  }

  getMainPhoto(userId: number) {
    return this.authors.get(userId)?.profilePhotoUrl ?? 'assets/photos/defaultPhoto.jpg';
  }

  addNewPostToggle() {
    const ref = this.modalService.show(AddPostComponent);
    ref.content?.saved.subscribe(() => this.loadFeed());
  }

  addNewCommentToggle(post: Post) {
    if (!this.user)
      return;

    const ref = this.modalService.show(AddCommentComponent, {
      initialState: { user: this.user, post }
    });
    ref.content?.saved.subscribe(() => this.loadFeed());
  }
}
