import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Gender } from 'src/app/core/models/Enums';
import { Pagination } from 'src/app/core/models/Pagination';
import { User } from 'src/app/core/models/User';
import { UserService } from 'src/app/core/services/user.service';

@Component({
    selector: 'app-search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.css'],
    standalone: false
})
export class SearchComponent implements OnInit {
  users: User[] = [];
  pagination: Pagination | null = null;
  pageNumber = 1;
  pageSize = 12;

  usernameFilter = '';
  selectedGender: Gender | null = null;
  countryFilter = '';
  cityFilter = '';

  readonly Gender = Gender;

  private usernameFilter$ = new Subject<string>();
  private countryFilter$ = new Subject<string>();
  private cityFilter$ = new Subject<string>();

  constructor(private userService: UserService) {
    this.usernameFilter$.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => this.onFilterChange());
    this.countryFilter$.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => this.onFilterChange());
    this.cityFilter$.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => this.onFilterChange());
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  onUsernameFilterChange(value: string) {
    this.usernameFilter = value;
    this.usernameFilter$.next(value);
  }

  onCountryFilterChange(value: string) {
    this.countryFilter = value;
    this.countryFilter$.next(value);
  }

  onCityFilterChange(value: string) {
    this.cityFilter = value;
    this.cityFilter$.next(value);
  }

  onFilterChange() {
    this.pageNumber = 1;
    this.loadUsers();
  }

  pageChanged(event: { page: number }) {
    this.pageNumber = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUserList({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      username: this.usernameFilter || undefined,
      gender: this.selectedGender ?? undefined,
      country: this.countryFilter || undefined,
      city: this.cityFilter || undefined
    }).subscribe({
      next: response => {
        this.users = response.result;
        this.pagination = response.pagination;
      }
    });
  }
}
