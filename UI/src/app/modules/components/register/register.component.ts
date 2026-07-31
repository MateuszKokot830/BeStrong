import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { RegisterRequest } from 'src/app/core/models/Auth';
import { AccountService } from 'src/app/core/services/account.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    standalone: false
})
export class RegisterComponent {
  model: RegisterRequest = { userName: '', password: '' };

  constructor(public bsModalRef: BsModalRef, private accountService: AccountService) {}

  register() {
    this.accountService.register(this.model).subscribe({
      next: _ => this.bsModalRef.hide()
    });
  }
}
