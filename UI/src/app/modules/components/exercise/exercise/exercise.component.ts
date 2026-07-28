import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ExerciseCreate } from 'src/app/core/models/Exercise';
import { MuscleSubgroup } from 'src/app/core/models/Enums';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-exercise',
  templateUrl: './exercise.component.html',
  styleUrls: ['./exercise.component.css']
})
export class ExerciseComponent {
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
    this.workoutService.addExercise(this.exercise).subscribe();
    location.reload();
    this.toastr.success('Exercise has been added!');
  }
}
