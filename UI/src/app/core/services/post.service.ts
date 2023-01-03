import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Comment } from '../models/Comment';
import { Post } from '../models/Post';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getFollowerPosts(ids: number[]) {
    var httpRequest = "";
    for(const id of ids) {
      httpRequest = httpRequest.concat("ids=", id.toString(), "&");
    };
    httpRequest.slice(0, -1);
    return this.http.get<Post[]>(this.baseUrl + 'posts/users/followers?' + httpRequest);
  }

  createNewPost(post: Post) {
    return this.http.post<Post>(this.baseUrl + 'posts', post);
  }

  addCommentToPost(comment: Comment) {
    return this.http.post<Comment>(this.baseUrl + 'posts/comments', comment);
  }
}
