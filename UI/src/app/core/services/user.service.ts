import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getUser(username: String) {
    return this.http.get<User>(this.baseUrl + 'Users' + username, this.getHttpOptions())
  }

  getUsers() {
    return this.http.get<User[]>(this.baseUrl + 'Users', this.getHttpOptions())
  }

  getHttpOptions() {
    const token = localStorage.getItem("user");
    if (!token) return;

    const user = JSON.parse(token);
    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + user.token
      })
    }
  }
}
