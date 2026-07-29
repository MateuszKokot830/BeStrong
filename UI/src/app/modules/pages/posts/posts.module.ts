import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { AddCommentComponent } from '../../components/add-comment/add-comment.component';
import { AddPostComponent } from '../../components/add-post/add-post.component';
import { PostsComponent } from './posts.component';

@NgModule({
  declarations: [
    PostsComponent,
    AddPostComponent,
    AddCommentComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([{ path: '', component: PostsComponent }])
  ]
})
export class PostsModule { }
