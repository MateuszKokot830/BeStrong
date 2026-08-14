import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { WorkoutPlan } from 'src/app/core/models/WorkoutPlan';

@Component({
    selector: 'app-workout-plan-details',
    templateUrl: './workout-plan-details.component.html',
    styleUrls: ['./workout-plan-details.component.css'],
    standalone: false
})
export class WorkoutPlanDetailsComponent {
  plan!: WorkoutPlan;

  constructor(public bsModalRef: BsModalRef) { }
}
