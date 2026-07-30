import { Component } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { LoginRequest } from 'src/app/core/models/Auth';
import { AccountService } from 'src/app/core/services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  model: LoginRequest = { userName: '', password: '' };
  isNavCollapsed = true;

  constructor(public accountService: AccountService, private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.isNavCollapsed = true;
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
    this.accountService.logout();
    this.router.navigateByUrl('');
  }
}
