import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { WorkoutsComponent } from './workouts.component';

@NgModule({
  declarations: [WorkoutsComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([{ path: '', component: WorkoutsComponent }])
  ]
})
export class WorkoutsModule { }
