import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { WorkoutPlanDetailsComponent } from './workout-plan-details/workout-plan-details.component';
import { WorkoutPlansComponent } from './workout-plans.component';

@NgModule({
  declarations: [WorkoutPlansComponent, WorkoutPlanDetailsComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([{ path: '', component: WorkoutPlansComponent }])
  ]
})
export class WorkoutPlansModule { }
