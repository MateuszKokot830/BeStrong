import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserApp } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getUser(username: String) {
    return this.http.get<UserApp>(this.baseUrl + 'users/' + username);
  }

  getUsers() {
    return this.http.get<UserApp[]>(this.baseUrl + 'users');
  }
}
