import { Component, OnInit } from '@angular/core';
import { Exercise } from 'src/app/core/models/Exercise';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrls: ['./workout.component.css']
})
export class WorkoutComponent implements OnInit {
  exercises: Exercise[] = [];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadExercises();
  }

  loadExercises() {
    this.userService.getExercises().subscribe({
      next: exercises => this.exercises = exercises
    })
  }

}
