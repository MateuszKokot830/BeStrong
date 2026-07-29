import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { WorkoutPlanComponent } from './workout-plan.component';

@NgModule({
  declarations: [WorkoutPlanComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([{ path: '', component: WorkoutPlanComponent }])
  ]
})
export class WorkoutPlanModule { }
