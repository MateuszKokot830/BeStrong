import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { AccountService } from './core/services/account.service';
import { UserAuth } from './core/models/Auth';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('AppComponent', () => {
  let accountService: AccountService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
    declarations: [AppComponent],
    schemas: [NO_ERRORS_SCHEMA],
    imports: [RouterTestingModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();

    accountService = TestBed.inject(AccountService);
    localStorage.removeItem('user');
  });

  afterEach(() => localStorage.removeItem('user'));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it(`should have as title 'BeStrong'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    expect(fixture.componentInstance.title).toEqual('BeStrong');
  });

  it('restores a stored user into the account service', () => {
    const stored: UserAuth = { username: 'mateusz', token: 'a.b.c' };
    localStorage.setItem('user', JSON.stringify(stored));

    const fixture = TestBed.createComponent(AppComponent);
    fixture.componentInstance.ngOnInit();

    let emitted: UserAuth | null | undefined;
    accountService.currentUser$.subscribe(user => emitted = user);
    expect(emitted).toEqual(stored);
  });

  it('emits null when nothing is stored, so guards do not hang', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.componentInstance.ngOnInit();

    let emitted: UserAuth | null | undefined;
    let didEmit = false;
    accountService.currentUser$.subscribe(user => {
      emitted = user;
      didEmit = true;
    });

    expect(didEmit).toBeTrue();
    expect(emitted).toBeNull();
  });
});
