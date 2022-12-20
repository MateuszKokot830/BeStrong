import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch(error.status) {
            case 400:
              if (error.error.errors) {
                for (const msg in error.error.errors) {
                  if (error.error.errors[msg]) {
                    this.toastr.error(error.error.errors[msg], error.status);
                  }
                }
              } else {
                this.toastr.error(error.error.title, error.status);
              }
              break;
            case 401:
              this.toastr.error(error.error.title, error.status);
              break;
            case 404:
              this.toastr.error(error.title, error.status);
              break;
            case 500:
              this.toastr.error(error.error.title, error.status);
              break;
            default:
              this.toastr.error('Something went wrong. Please refresh the page.');
              console.log(error);
              break;
          }
        }
        return throwError(error);
      })
    )
  }
}
