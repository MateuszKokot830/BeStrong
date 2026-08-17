import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Exercise, ExerciseCreate } from '../models/Exercise';
import { PaginatedResult, PaginationHeader } from '../models/Pagination';
import { Workout, WorkoutCreate, WorkoutCriteria } from '../models/Workout';
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

  getWorkouts(criteria: WorkoutCriteria) {
    let params = new HttpParams()
      .set('pageNumber', criteria.pageNumber)
      .set('pageSize', criteria.pageSize);

    if (criteria.dateFrom) {
      params = params.set('dateFrom', criteria.dateFrom);
    }

    if (criteria.dateTo) {
      params = params.set('dateTo', criteria.dateTo);
    }

    if (criteria.name) {
      params = params.set('name', criteria.name);
    }

    if (criteria.exerciseId !== undefined && criteria.exerciseId !== null) {
      params = params.set('exerciseId', criteria.exerciseId);
    }

    return this.http.get<Workout[]>(this.baseUrl + 'workouts', { observe: 'response', params }).pipe(
      map((response): PaginatedResult<Workout[]> => {
        const header = response.headers.get('Pagination');
        const parsed: PaginationHeader = header ? JSON.parse(header) : null;

        return {
          result: response.body ?? [],
          pagination: parsed
            ? {
                currentPage: parsed.currentPage,
                itemsPerPage: parsed.itemsPerPage,
                totalItems: parsed.totalItems,
                totalPages: parsed.totalPages
              }
            : null
        };
      })
    );
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
