import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginRequest } from 'src/app/core/models/Auth';
import { AccountService } from 'src/app/core/services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  model: LoginRequest = { userName: '', password: '' };

  constructor(public accountService: AccountService, private router: Router) { }

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
