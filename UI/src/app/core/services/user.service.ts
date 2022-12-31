import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { concat } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getUser(username: String) {
    return this.http.get<User>(this.baseUrl + 'users/' + username);
  }

  getUsers() {
    return this.http.get<User[]>(this.baseUrl + 'users');
  }

  updateUser(user: User) {
    return this.http.put(this.baseUrl + 'users', user);
  }

  getFollowerUsers(ids: number[]) {
    var httpRequest = "";
    for(const id of ids) {
      httpRequest = httpRequest.concat("ids=", id.toString(), "&");
    };
    httpRequest.slice(0, -1);
    return this.http.get<User[]>(this.baseUrl + 'users/followers?' + httpRequest);
  }

  followUser(id: number) {
    return this.http.put(this.baseUrl + 'users/followers/' + id,  id);
  }

  // getExercises() {
  //   return this.http.get<Exercise[]>(this.baseUrl + 'Workouts/Exercises');
  // }
}
