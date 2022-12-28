import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { Exercise } from 'src/app/core/models/Exercise';
import { UserAuth } from 'src/app/core/models/User';
import { Workout, WorkoutExercise } from 'src/app/core/models/Workout';
import { AccountService } from 'src/app/core/services/account.service';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrls: ['./workout.component.css']
})
export class WorkoutComponent implements OnInit {
  currentUser: UserAuth | null = null;
  exercises: Exercise[] = [];
  workout = {} as Workout;
  workoutExercise = {} as WorkoutExercise;
  exerciseCounter = 1;

  constructor(private workoutService: WorkoutService, private accountService: AccountService, private userService: UserService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: currentUser => this.currentUser = currentUser
    });
  }

  ngOnInit(): void {
    this.loadExercises();
    this.startWorkout();
  }

  loadExercises() {
    this.workoutService.getExercises().subscribe({
      next: exercises => this.exercises = exercises
    })
  }

  startWorkout() {
    this.workout.workoutExercises = [];
    this.userService.getUser(this.currentUser.username).subscribe({
      next: user => this.workout.userId = user.id
    });
  }

  addExercise() {
    var exerciseName = this.exercises.find(x => x.id == this.workoutExercise.exerciseId).name;
    var div = document.getElementById('exercise');
    var text = `<div class="card bg-dark mt-3">
      <div class="card-body">
        <h4 class="text-white text-center">Exercise ` + this.exerciseCounter.toString() + `
        </h4>
        <div class="row">
          <div class="col-4 mt-4">
            <h4 class="text-center">` + exerciseName + `</h4>
          </div>
          <div class="col-8 mt-2">
            <table cellPadding="0" id="setTable" cellspacing="0" style="margin:auto;">
              <thead>
                <tr class="text-white text-center">
                  <th scope="col" style="border: 3px solid grey; padding: 5px">SETS</th>
                  <th scope="col" style="border: 3px solid grey; padding: 5px">REPS / DURATION</th>
                  <th scope="col" style="border: 3px solid grey; padding: 5px">WEIGHT</th>
                </tr>
              </thead>
              <tbody>
                <tr class="text-white text-center">
                  <th scope="col" style="border: 3px solid grey; padding: 5px">` + this.workoutExercise.sets + `</th>
                  <th scope="col" style="border: 3px solid grey; padding: 5px">` + this.workoutExercise.reps + `</th>
                  <th scope="col" style="border: 3px solid grey; padding: 5px">` + this.workoutExercise.weight + ` kg</th>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
      </div>`;
    div.insertAdjacentHTML('beforeend', text);
    this.exerciseCounter += 1;
    var currentExercise = { ...this.workoutExercise }
    this.workout.workoutExercises.push(currentExercise);
  }

  addWorkout() {
    this.workoutService.addExercise(this.workout).subscribe();
    location.reload();
  }
}
