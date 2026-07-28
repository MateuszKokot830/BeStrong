import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/core/models/Pagination';
import { User } from 'src/app/core/models/User';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  users: User[] = [];
  pagination: Pagination | null = null;
  pageNumber = 1;
  pageSize = 10;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUserList({ pageNumber: this.pageNumber, pageSize: this.pageSize }).subscribe({
      next: response => {
        this.users = response.result;
        this.pagination = response.pagination;
      }
    });
  }

  pageChanged(event: { page: number }) {
    this.pageNumber = event.page;
    this.loadUsers();
  }
}
