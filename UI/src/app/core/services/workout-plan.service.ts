import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { WorkoutPlan, WorkoutPlanCategoryOption, WorkoutPlanCreate } from '../models/WorkoutPlan';

@Injectable({
  providedIn: 'root'
})
export class WorkoutPlanService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getCategories() {
    return this.http.get<WorkoutPlanCategoryOption[]>(this.baseUrl + 'workoutplans/categories');
  }

  addWorkoutPlan(plan: WorkoutPlanCreate) {
    return this.http.post<WorkoutPlan>(this.baseUrl + 'workoutplans', plan);
  }
}
