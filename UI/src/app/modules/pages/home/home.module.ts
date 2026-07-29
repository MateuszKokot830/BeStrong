import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { RegisterComponent } from '../../components/register/register.component';
import { HomeComponent } from './home.component';

@NgModule({
  declarations: [
    HomeComponent,
    RegisterComponent
  ],
  imports: [SharedModule],
  exports: [HomeComponent]
})

export class HomeModule { }
