import { Component, OnInit } from '@angular/core';
import { UserAuth } from './core/models/User';
import { AccountService } from './core/services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'BeStrong';
  users: any;

  constructor(private accountService: AccountService) {

  }

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: UserAuth = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }
}
