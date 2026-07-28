import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Exercise, ExerciseCreate } from '../models/Exercise';
import { Workout, WorkoutCreate } from '../models/Workout';
import { Statistics } from '../models/Statistics';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getUserWorkouts(userId: number) {
    return this.http.get<Workout[]>(`${this.baseUrl}workouts/${userId}`);
  }

  addWorkout(workout: WorkoutCreate) {
    return this.http.post<Workout>(this.baseUrl + 'workouts', workout);
  }

  updateWorkout(workoutId: number, workout: WorkoutCreate) {
    return this.http.put<Workout>(`${this.baseUrl}workouts/${workoutId}`, workout);
  }

  deleteWorkout(workoutId: number) {
    return this.http.delete(`${this.baseUrl}workouts/${workoutId}`);
  }

  getStatistics(userId: number) {
    return this.http.get<Statistics>(`${this.baseUrl}workouts/statistics/${userId}`);
  }

  calculate(weight: number, reps: number) {
    return this.http.get<number>(`${this.baseUrl}workouts/weight/${weight}/reps/${reps}`);
  }

  getExercises() {
    return this.http.get<Exercise[]>(this.baseUrl + 'exercises');
  }

  addExercise(exercise: ExerciseCreate) {
    return this.http.post<Exercise>(this.baseUrl + 'exercises', exercise);
  }
}
