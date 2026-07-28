import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Comment, CommentCreate, CommentUpdate } from '../models/Comment';
import { Post, PostCreate, PostUpdate } from '../models/Post';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getFeed() {
    return this.http.get<Post[]>(this.baseUrl + 'posts/feed');
  }

  getPosts() {
    return this.http.get<Post[]>(this.baseUrl + 'posts');
  }

  getPost(postId: number) {
    return this.http.get<Post>(`${this.baseUrl}posts/${postId}`);
  }

  createPost(post: PostCreate) {
    return this.http.post<Post>(this.baseUrl + 'posts', post);
  }

  updatePost(postId: number, post: PostUpdate) {
    return this.http.put<Post>(`${this.baseUrl}posts/${postId}`, post);
  }

  deletePost(postId: number) {
    return this.http.delete(`${this.baseUrl}posts/${postId}`);
  }

  addComment(comment: CommentCreate) {
    return this.http.post<Comment>(this.baseUrl + 'posts/comments', comment);
  }

  updateComment(commentId: number, comment: CommentUpdate) {
    return this.http.put<Comment>(`${this.baseUrl}posts/comments/${commentId}`, comment);
  }

  deleteComment(commentId: number) {
    return this.http.delete(`${this.baseUrl}posts/comments/${commentId}`);
  }

  likePost(postId: number) {
    return this.http.post(`${this.baseUrl}posts/${postId}/like`, null);
  }

  unlikePost(postId: number) {
    return this.http.delete(`${this.baseUrl}posts/${postId}/like`);
  }

  likeComment(commentId: number) {
    return this.http.post(`${this.baseUrl}posts/comments/${commentId}/like`, null);
  }

  unlikeComment(commentId: number) {
    return this.http.delete(`${this.baseUrl}posts/comments/${commentId}/like`);
  }
}
