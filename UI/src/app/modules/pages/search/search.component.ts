import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/core/models/Pagination';
import { User, UserAuth } from 'src/app/core/models/User';
import { UserService } from 'src/app/core/services/user.service';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from 'src/app/core/services/account.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  users: User[]= [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 10;
  currentUser: UserAuth | null = null;

  constructor(private userService: UserService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: currentUser => this.currentUser = currentUser
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUserList(this.pageNumber, this.pageSize).subscribe({
      next: response => {
        if (response.result && response.pagination) {
          this.users = response.result;
          this.pagination = response.pagination;
        }
      }
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadUsers();
  }

}
