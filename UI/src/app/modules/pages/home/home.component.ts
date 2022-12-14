import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { AccountService } from 'src/app/core/services/account.service';
import { RegisterComponent } from '../../components/register/register.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  bsModalRef: BsModalRef;

  constructor(private modalService: BsModalService, public accountService: AccountService) {}

  ngOnInit(): void {
  }

  registerToggle() {
    this.bsModalRef = this.modalService.show(RegisterComponent);
  }
}
