import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs/operators';
import { ReplaySubject } from 'rxjs';
import { LoginRequest, RegisterRequest, UserAuth } from '../models/Auth';
import { environment } from 'src/environments/environment';

const STORAGE_KEY = 'user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.baseUrl;
  private currentUserSource = new ReplaySubject<UserAuth | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(credentials: LoginRequest) {
    return this.http.post<UserAuth>(this.baseUrl + 'auth/login', credentials).pipe(
      tap(user => this.storeUser(user))
    );
  }

  register(credentials: RegisterRequest) {
    return this.http.post<UserAuth>(this.baseUrl + 'auth/register', credentials).pipe(
      tap(user => this.storeUser(user))
    );
  }

  logout() {
    localStorage.removeItem(STORAGE_KEY);
    this.currentUserSource.next(null);
  }

  setCurrentUser(user: UserAuth | null) {
    this.currentUserSource.next(user);
  }

  private storeUser(user: UserAuth) {
    if (!user)
      return;

    localStorage.setItem(STORAGE_KEY, JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
