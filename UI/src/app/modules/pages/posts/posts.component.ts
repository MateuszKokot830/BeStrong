import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { PostType } from 'src/app/core/models/Enums';
import { Post } from 'src/app/core/models/Post';
import { User } from 'src/app/core/models/User';
import { WorkoutExercise } from 'src/app/core/models/Workout';
import { AccountService } from 'src/app/core/services/account.service';
import { PostService } from 'src/app/core/services/post.service';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';
import { AddCommentComponent } from '../../components/add-comment/add-comment.component';
import { AddPostComponent } from '../../components/add-post/add-post.component';

@Component({
    selector: 'app-posts',
    templateUrl: './posts.component.html',
    styleUrls: ['./posts.component.css'],
    standalone: false
})
export class PostsComponent implements OnInit {
  readonly PostType = PostType;

  user: User | null = null;
  posts: Post[] = [];
  authors = new Map<number, User>();
  exerciseNames = new Map<number, string>();
  isLoading = false;

  constructor(private accountService: AccountService, private userService: UserService,
    private postService: PostService, private workoutService: WorkoutService, private modalService: BsModalService) { }

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
          ownPosts: this.userService.getUserPosts(user.userName),
          exercises: this.workoutService.getExercises()
        });
      })
    ).subscribe({
      next: ({ me, followedUsers, feed, ownPosts, exercises }) => {
        this.isLoading = false;
        this.user = me;
        this.authors = new Map(followedUsers.map(u => [u.id, u]));
        this.authors.set(me.id, me);
        this.exerciseNames = new Map(exercises.map(e => [e.id, e.name ?? 'Unknown exercise']));

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
