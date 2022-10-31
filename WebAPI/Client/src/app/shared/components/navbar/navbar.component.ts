import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/app/core/models/User';
import { AccountService } from 'src/app/core/services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model: any = {}

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  login()
  {
    this.accountService.login(this.model).subscribe(respone => {
      console.log(respone);
    }, error => {
      console.log(error);
    });
  }

  logout()
  {
    this.accountService.logout();
  }

}
