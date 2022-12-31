import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../models/Post';
import { User } from '../models/User';

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
}
