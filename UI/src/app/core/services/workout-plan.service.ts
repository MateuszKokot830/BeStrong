import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResult, PaginationHeader } from '../models/Pagination';
import { WorkoutPlan, WorkoutPlanCategoryOption, WorkoutPlanCreate, WorkoutPlanCriteria } from '../models/WorkoutPlan';

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

  getWorkoutPlan(id: number) {
    return this.http.get<WorkoutPlan>(this.baseUrl + 'workoutplans/' + id);
  }

  updateWorkoutPlan(id: number, plan: WorkoutPlanCreate) {
    return this.http.put<WorkoutPlan>(this.baseUrl + 'workoutplans/' + id, plan);
  }

  deleteWorkoutPlan(id: number) {
    return this.http.delete(this.baseUrl + 'workoutplans/' + id);
  }

  assignWorkoutPlan(id: number) {
    return this.http.post(this.baseUrl + 'workoutplans/' + id + '/assign', null);
  }

  unassignWorkoutPlan(id: number) {
    return this.http.delete(this.baseUrl + 'workoutplans/' + id + '/assign');
  }

  getWorkoutPlans(criteria: WorkoutPlanCriteria) {
    let params = new HttpParams()
      .set('pageNumber', criteria.pageNumber)
      .set('pageSize', criteria.pageSize);

    if (criteria.category !== undefined && criteria.category !== null) {
      params = params.set('category', criteria.category);
    }

    if (criteria.name) {
      params = params.set('name', criteria.name);
    }

    if (criteria.onlyOwn) {
      params = params.set('onlyOwn', criteria.onlyOwn);
    }

    return this.http.get<WorkoutPlan[]>(this.baseUrl + 'workoutplans', { observe: 'response', params }).pipe(
      map((response): PaginatedResult<WorkoutPlan[]> => {
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
}
