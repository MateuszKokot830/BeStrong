import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Exercise } from '../models/Exercise';
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

  getExercises() {
    return this.http.get<Exercise[]>(this.baseUrl + 'Workouts/Exercises');
  }
}
