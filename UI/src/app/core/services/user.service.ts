import { HttpClient, HttpHandler, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../models/Pagination';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.baseUrl;
  paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

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

  followUser(userId: number, id: number) {
    return this.http.put(this.baseUrl + 'users/followers/' + id + "?userId=" + userId,  {userId, id});
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

  getUserList(page?: number, itemsPerPage?: number) {
    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<User[]>(this.baseUrl + 'users/list', {observe: 'response', params}).pipe(
      map(response => {
        if (response.body) {
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult;
      })
    );
  }
}
