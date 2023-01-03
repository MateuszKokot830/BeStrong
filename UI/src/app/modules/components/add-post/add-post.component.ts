import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Post } from 'src/app/core/models/Post';
import { User } from 'src/app/core/models/User';
import { PostService } from 'src/app/core/services/post.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.css']
})
export class AddPostComponent implements OnInit{
  user: User;
  post = {} as Post;
  // fileName = "";
  // formData: FormData;
  // photoAdded = false;

  constructor(public bsModalRef: BsModalRef, private postService: PostService,
    private toastr: ToastrService, private userService: UserService) { }

  ngOnInit(): void {
    this.post.userId = this.user.id;
  }

  addNewPost() {
    // if (this.photoAdded) {
    //   this.userService.addPhoto(this.formData, this.user.id).subscribe();
    //   this.post.photoId = this.user.photos[this.user.photos.length - 1].id;
    // }
    this.postService.createNewPost(this.post).subscribe({
      next: _ => {
        location.reload();
        this.toastr.success('Post has been added!');
      }
    });
  }


}
