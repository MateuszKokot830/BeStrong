import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserApp } from 'src/app/core/models/User';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  userApp: UserApp | undefined;

  constructor(private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;

    this.userService.getUser(username).subscribe({
      next: user => this.userApp = user
    });
  }

}
