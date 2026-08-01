import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorInterceptor } from './error.interceptor';
import { AccountService } from '../services/account.service';

describe('ErrorInterceptor', () => {
  let http: HttpClient;
  let httpMock: HttpTestingController;
  let accountService: jasmine.SpyObj<AccountService>;
  let router: jasmine.SpyObj<Router>;
  let toastr: jasmine.SpyObj<ToastrService>;

  beforeEach(() => {
    accountService = jasmine.createSpyObj('AccountService', ['logout']);
    router = jasmine.createSpyObj('Router', ['navigateByUrl']);
    toastr = jasmine.createSpyObj('ToastrService', ['error']);

    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting(),
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: AccountService, useValue: accountService },
        { provide: Router, useValue: router },
        { provide: ToastrService, useValue: toastr }
      ]
    });

    http = TestBed.inject(HttpClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('logs out, redirects home, and shows a session-expired toast on a 401 from a protected endpoint', () => {
    http.get('https://localhost:5001/api/users/me').subscribe({ error: () => {} });

    httpMock.expectOne('https://localhost:5001/api/users/me')
      .flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });

    expect(accountService.logout).toHaveBeenCalled();
    expect(router.navigateByUrl).toHaveBeenCalledWith('/');
    expect(toastr.error).toHaveBeenCalledWith('Your session has expired. Please log in again.');
  });

  it('does not log out on a 401 from the login endpoint, and shows the server message instead', () => {
    http.post('https://localhost:5001/api/auth/login', {}).subscribe({ error: () => {} });

    httpMock.expectOne('https://localhost:5001/api/auth/login')
      .flush('Invalid username or password', { status: 401, statusText: 'Unauthorized' });

    expect(accountService.logout).not.toHaveBeenCalled();
    expect(router.navigateByUrl).not.toHaveBeenCalled();
    expect(toastr.error).toHaveBeenCalledWith('Invalid username or password');
  });

  it('does not log out on a 401 from the register endpoint', () => {
    http.post('https://localhost:5001/api/auth/register', {}).subscribe({ error: () => {} });

    httpMock.expectOne('https://localhost:5001/api/auth/register')
      .flush('Username already taken', { status: 401, statusText: 'Unauthorized' });

    expect(accountService.logout).not.toHaveBeenCalled();
    expect(router.navigateByUrl).not.toHaveBeenCalled();
  });

  it('still shows the generic error toast for a non-401 failure', () => {
    http.get('https://localhost:5001/api/users/me').subscribe({ error: () => {} });

    httpMock.expectOne('https://localhost:5001/api/users/me')
      .flush('Server exploded', { status: 500, statusText: 'Internal Server Error' });

    expect(accountService.logout).not.toHaveBeenCalled();
    expect(toastr.error).toHaveBeenCalledWith('Server exploded');
  });
});
