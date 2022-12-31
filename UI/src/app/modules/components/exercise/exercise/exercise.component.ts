import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Exercise } from 'src/app/core/models/Exercise';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-exercise',
  templateUrl: './exercise.component.html',
  styleUrls: ['./exercise.component.css']
})
export class ExerciseComponent implements OnInit {
  exercise = {} as Exercise;

  constructor(public bsModalRef: BsModalRef, private workoutService: WorkoutService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  addExercise() {
    this.workoutService.addExercise(this.exercise).subscribe();
    location.reload();
    this.toastr.success('Exercise has been added!');
  }
}
