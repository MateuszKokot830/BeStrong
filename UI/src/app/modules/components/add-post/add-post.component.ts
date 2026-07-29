import { Component, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Post, PostCreate } from 'src/app/core/models/Post';
import { PostType } from 'src/app/core/models/Enums';
import { PostService } from 'src/app/core/services/post.service';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.css']
})
export class AddPostComponent {
  saved = new EventEmitter<Post>();
  isSaving = false;

  post: PostCreate = {
    type: PostType.Normal,
    description: null,
    workoutId: null,
    workoutPlan: null
  };

  constructor(public bsModalRef: BsModalRef, private postService: PostService,
    private toastr: ToastrService) { }

  addNewPost() {
    this.isSaving = true;
    this.postService.createPost(this.post).subscribe({
      next: post => {
        this.isSaving = false;
        this.toastr.success('Post has been added!');
        this.saved.emit(post);
        this.bsModalRef.hide();
      },
      error: _ => this.isSaving = false
    });
  }
}
