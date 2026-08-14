import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { WorkoutPlanCategory } from 'src/app/core/models/Enums';
import { Pagination } from 'src/app/core/models/Pagination';
import { WorkoutPlan, WorkoutPlanCategoryOption } from 'src/app/core/models/WorkoutPlan';
import { AccountService } from 'src/app/core/services/account.service';
import { WorkoutPlanService } from 'src/app/core/services/workout-plan.service';
import { WorkoutPlanDetailsComponent } from './workout-plan-details/workout-plan-details.component';

@Component({
    selector: 'app-workout-plans',
    templateUrl: './workout-plans.component.html',
    styleUrls: ['./workout-plans.component.css'],
    standalone: false
})
export class WorkoutPlansComponent implements OnInit {
  plans: WorkoutPlan[] = [];
  pagination: Pagination | null = null;
  pageNumber = 1;
  pageSize = 12;
  categories: WorkoutPlanCategoryOption[] = [];
  selectedCategory: WorkoutPlanCategory | null = null;
  nameFilter = '';
  onlyOwnFilter = false;
  currentUserId: number | null = null;
  currentPlanId: number | null = null;

  private nameFilter$ = new Subject<string>();

  constructor(
    private workoutPlanService: WorkoutPlanService,
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private modalService: BsModalService
  ) {
    this.nameFilter$.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => {
      this.pageNumber = 1;
      this.loadPlans();
    });
  }

  ngOnInit(): void {
    this.workoutPlanService.getCategories().subscribe(categories => this.categories = categories);
    this.accountService.currentProfile().subscribe(user => {
      this.currentUserId = user.id;
      this.currentPlanId = user.workoutPlanId;
    });
    this.loadPlans();
  }

  onNameFilterChange(value: string) {
    this.nameFilter = value;
    this.nameFilter$.next(value);
  }

  onCategoryChange() {
    this.pageNumber = 1;
    this.loadPlans();
  }

  onOnlyOwnChange() {
    this.pageNumber = 1;
    this.loadPlans();
  }

  pageChanged(event: { page: number }) {
    this.pageNumber = event.page;
    this.loadPlans();
  }

  loadPlans() {
    this.workoutPlanService.getWorkoutPlans({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      category: this.selectedCategory ?? undefined,
      name: this.nameFilter || undefined,
      onlyOwn: this.onlyOwnFilter || undefined
    }).subscribe(response => {
      this.plans = response.result;
      this.pagination = response.pagination;
    });
  }

  isOwn(plan: WorkoutPlan): boolean {
    return plan.createdById === this.currentUserId;
  }

  isCurrent(plan: WorkoutPlan): boolean {
    return plan.id === this.currentPlanId;
  }

  viewDetails(plan: WorkoutPlan) {
    this.modalService.show(WorkoutPlanDetailsComponent, {
      initialState: { plan },
      class: 'modal-lg'
    });
  }

  assign(plan: WorkoutPlan) {
    if (this.isCurrent(plan))
      return;

    const message = this.currentPlanId
      ? `Switch your active plan to "${plan.name}"? This will replace your current plan.`
      : `Set "${plan.name}" as your active workout plan?`;

    if (!confirm(message))
      return;

    this.workoutPlanService.assignWorkoutPlan(plan.id).subscribe({
      next: _ => {
        this.currentPlanId = plan.id;
        this.accountService.refreshProfile().subscribe();
        this.toastr.success(`"${plan.name}" is now your active plan.`);
        this.loadPlans();
      }
    });
  }

  deactivate(plan: WorkoutPlan) {
    if (!confirm(`Stop using "${plan.name}"? You won't have an active workout plan.`))
      return;

    this.workoutPlanService.unassignWorkoutPlan(plan.id).subscribe({
      next: _ => {
        this.currentPlanId = null;
        this.accountService.refreshProfile().subscribe();
        this.toastr.success(`"${plan.name}" is no longer your active plan.`);
        this.loadPlans();
      }
    });
  }

  edit(plan: WorkoutPlan) {
    this.router.navigate(['/workout-plan/edit', plan.id]);
  }

  delete(plan: WorkoutPlan) {
    if (plan.usedBy.length > 0) {
      this.toastr.error('This plan is in use and cannot be deleted.');
      return;
    }

    if (!confirm(`Delete "${plan.name}"? This cannot be undone.`))
      return;

    this.workoutPlanService.deleteWorkoutPlan(plan.id).subscribe({
      next: _ => {
        this.plans = this.plans.filter(p => p.id !== plan.id);
        this.toastr.success('Workout plan deleted.');
      }
    });
  }
}
