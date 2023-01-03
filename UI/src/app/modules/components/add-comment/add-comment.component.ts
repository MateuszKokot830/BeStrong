import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Comment } from 'src/app/core/models/Comment';
import { Post } from 'src/app/core/models/Post';
import { User } from 'src/app/core/models/User';
import { PostService } from 'src/app/core/services/post.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css']
})
export class AddCommentComponent implements OnInit {
  user: User;
  post: Post;
  comment = {} as Comment;

  constructor(public bsModalRef: BsModalRef, private postService: PostService,
    private toastr: ToastrService, private userService: UserService) { }

  ngOnInit(): void {
    this.comment.userId = this.user.id;
    this.comment.postId = this.post.id;
  }

  addComment() {
    this.postService.addCommentToPost(this.comment).subscribe({
      next: _ => {
        location.reload();
        this.toastr.success('Comment added to post!');
      }
    });
  }

}
