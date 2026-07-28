import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Exercise } from 'src/app/core/models/Exercise';
import { WorkoutCreate, WorkoutExercise } from 'src/app/core/models/Workout';
import { WorkoutService } from 'src/app/core/services/workout.service';
import { ExerciseComponent } from '../../components/exercise/exercise/exercise.component';

interface ExerciseDraft {
  exerciseId: number;
  sets: number;
  reps: number;
  weight: number;
}

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrls: ['./workout.component.css']
})
export class WorkoutComponent implements OnInit {
  exercises: Exercise[] = [];
  workoutName = '';
  drafts: ExerciseDraft[] = [];
  workoutExercise = {} as ExerciseDraft;
  exerciseCounter = 1;

  constructor(private workoutService: WorkoutService, private toastr: ToastrService,
    private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadExercises();
  }

  loadExercises() {
    this.workoutService.getExercises().subscribe({
      next: exercises => this.exercises = exercises
    });
  }

  addNewExerciseToggle() {
    this.modalService.show(ExerciseComponent);
  }

  addExercise() {
    var exerciseName = this.exercises.find(x => x.id == this.workoutExercise.exerciseId)?.name;
    var div = document.getElementById('exercise');
    if (!exerciseName || !div) return;

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
    this.drafts.push({ ...this.workoutExercise });
  }

  addWorkout() {
    const workout: WorkoutCreate = {
      name: this.workoutName,
      exercises: this.drafts.map((draft, index) => this.toWorkoutExercise(draft, index))
    };

    this.workoutService.addWorkout(workout).subscribe();
    location.reload();
    this.toastr.success('Workout has been saved!');
  }

  private toWorkoutExercise(draft: ExerciseDraft, index: number): WorkoutExercise {
    const setCount = Number(draft.sets) || 0;

    return {
      order: index + 1,
      notes: null,
      exerciseId: Number(draft.exerciseId),
      workoutId: 0,
      maxTotalWeight: null,
      bestEstimatedOneRepMax: null,
      sets: Array.from({ length: setCount }, (_, i) => ({
        setNumber: i + 1,
        reps: Number(draft.reps) || 0,
        weight: Number(draft.weight) || 0,
        totalWeight: null,
        estimatedOneRepMax: null
      }))
    };
  }
}
