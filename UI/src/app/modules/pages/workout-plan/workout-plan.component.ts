import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Exercise } from 'src/app/core/models/Exercise';
import { UserAuth } from 'src/app/core/models/User';
import { Workout, WorkoutExercise } from 'src/app/core/models/Workout';
import { AccountService } from 'src/app/core/services/account.service';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-workout-plan',
  templateUrl: './workout-plan.component.html',
  styleUrls: ['./workout-plan.component.css']
})
export class WorkoutPlanComponent implements OnInit {
  currentUser: UserAuth | null = null;
  exercises: Exercise[] = [];
  workout = {} as Workout;
  workoutExercise = {} as WorkoutExercise;
  exerciseCounter = 1;

  constructor(private workoutService: WorkoutService, private accountService: AccountService,
    private userService: UserService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: currentUser => this.currentUser = currentUser
    })
  }

  ngOnInit(): void {
  }

}
