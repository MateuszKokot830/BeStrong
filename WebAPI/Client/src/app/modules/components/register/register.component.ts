import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AccountService } from 'src/app/core/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};

  constructor(public bsModalRef: BsModalRef, private accountService: AccountService) {}

  ngOnInit(): void { 

  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.bsModalRef.hide();
    }, error => {
      console.log(error);
    })
  }

}
