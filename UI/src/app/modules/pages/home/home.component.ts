import { Component } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { AccountService } from 'src/app/core/services/account.service';
import { RegisterComponent } from '../../components/register/register.component';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: false
})
export class HomeComponent {
  constructor(private modalService: BsModalService, public accountService: AccountService) {}

  registerToggle() {
    this.modalService.show(RegisterComponent);
  }
}
