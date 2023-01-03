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

  addPhoto(file: FormData, id: number) {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', '*/*');
    return this.http.put(this.baseUrl + "users/" + id + "/photos", file, {headers: headers});
  }

  setMainPhoto(photoId: number, id: number)
  {
    return this.http.put(this.baseUrl + "users/" + id + "/photos/" + photoId, {photoId, id});
  }

  deletePhoto(photoId: number, id: number)
  {
    return this.http.delete(this.baseUrl + "users/" + id + "/photos/" + photoId);
  }
}
