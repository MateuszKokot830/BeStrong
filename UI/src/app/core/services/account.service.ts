import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { shareReplay, tap } from 'rxjs/operators';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { LoginRequest, RegisterRequest, UserAuth } from '../models/Auth';
import { User } from '../models/User';
import { environment } from 'src/environments/environment';
import { WorkoutDraftService } from './workout-draft.service';

const STORAGE_KEY = 'user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.baseUrl;
  private currentUserSource = new ReplaySubject<UserAuth | null>(1);
  currentUser$ = this.currentUserSource.asObservable();
  private currentProfileSource = new BehaviorSubject<User | null>(null);
  currentProfile$ = this.currentProfileSource.asObservable();
  private profileRequest?: Observable<User>;

  constructor(private http: HttpClient, private workoutDraft: WorkoutDraftService) { }

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
    this.currentProfileSource.next(null);
    this.invalidateProfile();
    this.workoutDraft.clear();
  }

  setCurrentUser(user: UserAuth | null) {
    this.currentUserSource.next(user);
  }

  currentProfile(): Observable<User> {
    if (!this.profileRequest) {
      this.profileRequest = this.http.get<User>(this.baseUrl + 'users/me').pipe(
        tap(user => this.currentProfileSource.next(user)),
        shareReplay({ bufferSize: 1, refCount: false })
      );
    }
    return this.profileRequest;
  }

  invalidateProfile() {
    this.profileRequest = undefined;
  }

  refreshProfile(): Observable<User> {
    this.invalidateProfile();
    return this.currentProfile();
  }

  private storeUser(user: UserAuth) {
    if (!user)
      return;

    localStorage.setItem(STORAGE_KEY, JSON.stringify(user));
    this.currentUserSource.next(user);
    this.invalidateProfile();
  }
}
