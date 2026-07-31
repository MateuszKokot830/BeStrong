import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ToastrModule } from 'ngx-toastr';
import { NavbarComponent } from './components/navbar/navbar.component';
import { UserCardComponent } from './components/user-card/user-card.component';

@NgModule({
  declarations: [
    NavbarComponent,
    UserCardComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    BsDropdownModule,
    CollapseModule,
    ModalModule,
    PaginationModule,
    TabsModule,
    ToastrModule
  ],
  exports: [
    NavbarComponent,
    UserCardComponent,
    CommonModule,
    FormsModule,
    RouterModule,
    BsDropdownModule,
    CollapseModule,
    ModalModule,
    PaginationModule,
    TabsModule,
    ToastrModule
  ]
})
export class SharedModule { }
