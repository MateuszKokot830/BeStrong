import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Exercise } from '../models/Exercise';
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

  addExercise(workout: Workout) {
    return this.http.post<Workout>(this.baseUrl + 'workouts', workout);
  }
}
