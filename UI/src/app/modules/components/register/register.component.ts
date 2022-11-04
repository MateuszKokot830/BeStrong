import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/core/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};

  constructor(public bsModalRef: BsModalRef, private accountService: AccountService,
    private toastr: ToastrService) {}

  ngOnInit(): void { 

  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.bsModalRef.hide();
    })
  }

}
