import { Component } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { LoginRequest } from 'src/app/core/models/Auth';
import { AccountService } from 'src/app/core/services/account.service';
import { WorkoutDraftService } from 'src/app/core/services/workout-draft.service';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css'],
    standalone: false
})
export class NavbarComponent {
  model: LoginRequest = { userName: '', password: '' };
  isNavCollapsed = true;

  constructor(public accountService: AccountService, private router: Router,
    private workoutDraft: WorkoutDraftService) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.isNavCollapsed = true;
      }
    });

    this.accountService.currentUser$.subscribe(user => {
      if (user) {
        this.accountService.currentProfile().subscribe();
      }
    });
  }

  toggleNav() {
    this.isNavCollapsed = !this.isNavCollapsed;
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('')
    });
  }

  logout() {
    if (!this.workoutDraft.isEmpty() &&
        !confirm('You have an unsaved workout in progress. Logging out will lose it. Continue?')) {
      return;
    }

    this.accountService.logout();
    this.router.navigateByUrl('');
  }
}
