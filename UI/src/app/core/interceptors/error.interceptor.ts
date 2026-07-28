import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

interface ValidationProblem {
  errors?: Record<string, string[]>;
}

interface Problem {
  title?: string;
  detail?: string;
}

interface ApiException {
  message?: string;
}

const GENERIC_MESSAGE = 'Something went wrong. Please refresh the page.';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        this.notify(error);
        return throwError(error);
      })
    );
  }

  private notify(error: HttpErrorResponse) {
    if (error.status === 0) {
      this.toastr.error('Could not reach the server. Check your connection.');
      return;
    }

    const body = error.error;

    if (typeof body === 'string' && body.trim()) {
      this.toastr.error(body);
      return;
    }

    const messages = this.validationMessages(body);
    if (messages.length) {
      messages.forEach(message => this.toastr.error(message));
      return;
    }

    const problem = body as (Problem & ApiException) | null;
    const message = problem?.detail || problem?.message || problem?.title;

    this.toastr.error(message || GENERIC_MESSAGE);
  }

  private validationMessages(body: unknown): string[] {
    const errors = (body as ValidationProblem | null)?.errors;
    if (!errors) return [];

    return Object.keys(errors).reduce<string[]>(
      (all, key) => all.concat(errors[key] ?? []), []);
  }
}
