import { Component, OnInit } from '@angular/core';
import { UserAuth } from './core/models/Auth';
import { AccountService } from './core/services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'BeStrong';

  constructor(private accountService: AccountService) { }

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const stored = localStorage.getItem('user');
    // Emit even when there is nothing stored: AuthGuard waits on currentUser$,
    // so skipping the emission would leave guarded routes hanging forever.
    const user: UserAuth | null = stored ? JSON.parse(stored) : null;
    this.accountService.setCurrentUser(user);
  }
}
