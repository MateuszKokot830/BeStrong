import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Exercise } from '../models/Exercise';
import { User } from '../models/User';
import { Workout } from '../models/Workout';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  // getWorkouts() {
  //   //return this.http.get<Workout[]>(this.baseUrl + 'users');
  // }

  getExercises() {
    return this.http.get<Exercise[]>(this.baseUrl + 'workouts/exercises');
  }

  addWorkout(workout: Workout) {
    return this.http.post<Workout>(this.baseUrl + 'workouts', workout);
  }

  addExercise(exercise: Exercise) {
    return this.http.post<Exercise>(this.baseUrl + 'workouts/exercises', exercise);
  }

  getUserWorkouts(user: User) {
    return this.http.get<Workout[]>(this.baseUrl + 'workouts/' + user.id);
  }
}
