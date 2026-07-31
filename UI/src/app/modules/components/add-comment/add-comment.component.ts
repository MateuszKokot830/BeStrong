import { Component, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Comment, CommentCreate } from 'src/app/core/models/Comment';
import { Post } from 'src/app/core/models/Post';
import { User } from 'src/app/core/models/User';
import { PostService } from 'src/app/core/services/post.service';

@Component({
    selector: 'app-add-comment',
    templateUrl: './add-comment.component.html',
    styleUrls: ['./add-comment.component.css'],
    standalone: false
})
export class AddCommentComponent {
  user!: User;
  post!: Post;
  saved = new EventEmitter<Comment>();
  isSaving = false;

  comment: CommentCreate = { description: null, postId: 0 };

  constructor(public bsModalRef: BsModalRef, private postService: PostService,
    private toastr: ToastrService) { }

  addComment() {
    this.isSaving = true;
    this.postService.addComment({ description: this.comment.description, postId: this.post.id }).subscribe({
      next: comment => {
        this.isSaving = false;
        this.toastr.success('Comment added to post!');
        this.saved.emit(comment);
        this.bsModalRef.hide();
      },
      error: _ => this.isSaving = false
    });
  }
}
