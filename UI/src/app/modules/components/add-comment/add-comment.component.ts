import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { CommentCreate } from 'src/app/core/models/Comment';
import { Post } from 'src/app/core/models/Post';
import { User } from 'src/app/core/models/User';
import { PostService } from 'src/app/core/services/post.service';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css']
})
export class AddCommentComponent {
  user!: User;
  post!: Post;
  comment: CommentCreate = { description: null, postId: 0 };

  constructor(public bsModalRef: BsModalRef, private postService: PostService,
    private toastr: ToastrService) { }

  addComment() {
    this.postService.addComment({ description: this.comment.description, postId: this.post.id }).subscribe({
      next: _ => {
        location.reload();
        this.toastr.success('Comment added to post!');
      }
    });
  }
}
