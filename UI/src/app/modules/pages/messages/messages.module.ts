import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { MessagesComponent } from './messages.component';

@NgModule({
  declarations: [MessagesComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([{ path: '', component: MessagesComponent }])
  ]
})
export class MessagesModule { }
