import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Exercise } from '../models/Exercise';
import { User } from '../models/User';
import { Workout } from '../models/Workout';
import { Statistics } from '../models/Statistics';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

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

  getStatistics(user: User) {
    return this.http.get<Statistics>(this.baseUrl + 'workouts/statistics/' + user.id);
  }

  calculate(weight: number, reps: number) {
    return this.http.get<number>(this.baseUrl + 'workouts/weight/' + weight + '/reps/' + reps);
  }
}
