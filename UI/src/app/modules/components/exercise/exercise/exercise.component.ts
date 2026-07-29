import { Component, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Exercise, ExerciseCreate } from 'src/app/core/models/Exercise';
import { MuscleSubgroup } from 'src/app/core/models/Enums';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-exercise',
  templateUrl: './exercise.component.html',
  styleUrls: ['./exercise.component.css']
})
export class ExerciseComponent {
  saved = new EventEmitter<Exercise>();
  isSaving = false;

  exercise: ExerciseCreate = {
    name: '',
    description: null,
    muscleSubgroup: MuscleSubgroup.Chest,
    imageUrl: null
  };

  muscleSubgroups = Object.keys(MuscleSubgroup)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ value: MuscleSubgroup[key as keyof typeof MuscleSubgroup], label: key }));

  constructor(public bsModalRef: BsModalRef, private workoutService: WorkoutService,
    private toastr: ToastrService) { }

  addExercise() {
    this.isSaving = true;
    this.workoutService.addExercise(this.exercise).subscribe({
      next: exercise => {
        this.isSaving = false;
        this.toastr.success('Exercise has been added!');
        this.saved.emit(exercise);
        this.bsModalRef.hide();
      },
      error: _ => this.isSaving = false
    });
  }
}
