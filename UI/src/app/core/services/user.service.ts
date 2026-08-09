import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResult, PaginationHeader, UserSearchCriteria } from '../models/Pagination';
import { Post } from '../models/Post';
import { User, UserUpdate } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }


  getUser(username: string) {
    return this.http.get<User>(`${this.baseUrl}users/${username}`);
  }

  getUserPosts(username: string) {
    return this.http.get<Post[]>(`${this.baseUrl}users/${username}/posts`);
  }

  getUsers() {
    return this.http.get<User[]>(this.baseUrl + 'users');
  }

  updateUser(user: UserUpdate) {
    return this.http.put<User>(this.baseUrl + 'users', user);
  }

  getUsersByIds(ids: number[]) {
    const params = ids.reduce((acc, id) => acc.append('ids', id), new HttpParams());
    return this.http.get<User[]>(this.baseUrl + 'users/followers', { params });
  }

  followUser(userId: number, followUserId: number) {
    const params = new HttpParams().set('followUserId', followUserId);
    return this.http.post(`${this.baseUrl}users/${userId}/follow`, null, { params });
  }

  unfollowUser(userId: number, unfollowUserId: number) {
    const params = new HttpParams().set('unfollowUserId', unfollowUserId);
    return this.http.delete(`${this.baseUrl}users/${userId}/follow`, { params });
  }

  addPhoto(file: FormData, userId: number) {
    return this.http.put(`${this.baseUrl}users/${userId}/photos`, file);
  }

  setMainPhoto(photoId: number, userId: number) {
    return this.http.put(`${this.baseUrl}users/${userId}/photos/${photoId}`, null);
  }

  deletePhoto(photoId: number, userId: number) {
    return this.http.delete(`${this.baseUrl}users/${userId}/photos/${photoId}`);
  }

  getUserList(criteria: UserSearchCriteria) {
    let params = new HttpParams()
      .set('pageNumber', criteria.pageNumber)
      .set('pageSize', criteria.pageSize);

    if (criteria.excludeUsername) {
      params = params.set('excludeUsername', criteria.excludeUsername);
    }

    return this.http.get<User[]>(this.baseUrl + 'users/list', { observe: 'response', params }).pipe(
      map((response): PaginatedResult<User[]> => {
        const header = response.headers.get('Pagination');
        const parsed: PaginationHeader = header ? JSON.parse(header) : null;

        return {
          result: response.body ?? [],
          pagination: parsed
            ? {
                currentPage: parsed.currentPage,
                itemsPerPage: parsed.itemsPerPage,
                totalItems: parsed.totalItems,
                totalPages: parsed.totalPages
              }
            : null
        };
      })
    );
  }
}
