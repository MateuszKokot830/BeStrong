import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models/User';

@Component({
    selector: 'app-user-card',
    templateUrl: './user-card.component.html',
    styleUrls: ['./user-card.component.css'],
    standalone: false
})
export class UserCardComponent {
  @Input() user!: User;

  constructor(private router: Router) { }

  goToProfile(username: string) {
    this.router.navigate(['/profile/' + username]);
  }
}
