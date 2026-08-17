import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Exercise } from 'src/app/core/models/Exercise';
import { Pagination } from 'src/app/core/models/Pagination';
import { Workout, WorkoutExercise } from 'src/app/core/models/Workout';
import { WorkoutPlan } from 'src/app/core/models/WorkoutPlan';
import { AccountService } from 'src/app/core/services/account.service';
import { DraftExercise, WorkoutDraftService } from 'src/app/core/services/workout-draft.service';
import { WorkoutPlanService } from 'src/app/core/services/workout-plan.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
    selector: 'app-workouts',
    templateUrl: './workouts.component.html',
    styleUrls: ['./workouts.component.css'],
    standalone: false
})
export class WorkoutsComponent implements OnInit {
  workouts: Workout[] = [];
  pagination: Pagination | null = null;
  pageNumber = 1;
  pageSize = 12;
  exercises: Exercise[] = [];
  exerciseNames = new Map<number, string>();
  dateFrom: string = firstOfCurrentMonth();
  dateTo: string | null = null;
  nameFilter = '';
  selectedExerciseId: number | null = null;
  activeWorkoutPlan: WorkoutPlan | null = null;
  selectedTemplateOrder: number | null = null;

  private nameFilter$ = new Subject<string>();

  constructor(
    private workoutService: WorkoutService,
    private workoutPlanService: WorkoutPlanService,
    public accountService: AccountService,
    private draft: WorkoutDraftService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.nameFilter$.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => {
      this.pageNumber = 1;
      this.loadWorkouts();
    });
  }

  ngOnInit(): void {
    this.workoutService.getExercises().subscribe(exercises => {
      this.exercises = exercises;
      this.exerciseNames = new Map(exercises.map(e => [e.id, e.name ?? 'Unknown exercise']));
    });

    this.accountService.currentProfile().subscribe(user => {
      if (user.workoutPlanId) {
        this.workoutPlanService.getWorkoutPlan(user.workoutPlanId).subscribe(plan => this.activeWorkoutPlan = plan);
      }
    });

    this.loadWorkouts();
  }

  onNameFilterChange(value: string) {
    this.nameFilter = value;
    this.nameFilter$.next(value);
  }

  onFilterChange() {
    this.pageNumber = 1;
    this.loadWorkouts();
  }

  pageChanged(event: { page: number }) {
    this.pageNumber = event.page;
    this.loadWorkouts();
  }

  loadWorkouts() {
    this.workoutService.getWorkouts({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      dateFrom: this.dateFrom || undefined,
      dateTo: this.dateTo || undefined,
      name: this.nameFilter || undefined,
      exerciseId: this.selectedExerciseId ?? undefined
    }).subscribe(response => {
      this.workouts = response.result;
      this.pagination = response.pagination;
    });
  }

  getExerciseName(exerciseId: number) {
    return this.exerciseNames.get(exerciseId) ?? 'Unknown exercise';
  }

  formatReps(exercise: WorkoutExercise) {
    const reps = exercise.sets.map(s => s.reps);
    if (!reps.length)
      return '-';

    return reps.every(r => r === reps[0])
      ? `${reps.length}x${reps[0]}`
      : reps.join(' / ');
  }

  formatWeight(exercise: WorkoutExercise) {
    const weights = exercise.sets.map(s => s.weight ?? 0);
    if (!weights.length)
      return '-';

    return weights.every(w => w === weights[0])
      ? `${weights[0]} kg`
      : `${weights.join(' / ')} kg`;
  }

  startEmptyWorkout() {
    this.router.navigate(['/workout']);
  }

  copyWorkout(workout: Workout) {
    if (!this.draft.isEmpty() && !confirm('This will replace your current unsaved workout. Continue?'))
      return;

    const draftExercises: DraftExercise[] = [];
    for (const workoutExercise of workout.workoutExercises) {
      const exercise = this.exercises.find(e => e.id === workoutExercise.exerciseId);
      if (!exercise)
        continue;

      draftExercises.push({
        exerciseId: exercise.id,
        name: exercise.name,
        imageUrl: exercise.imageUrl,
        muscleSubgroup: exercise.muscleSubgroup,
        notes: workoutExercise.notes,
        sets: workoutExercise.sets.map(s => ({ reps: s.reps, weight: s.weight }))
      });
    }

    this.draft.copyFromWorkout(workout.name, draftExercises);
    this.toastr.success(`Copied "${workout.name || 'workout'}"`);
    this.router.navigate(['/workout']);
  }

  onCopyTemplateSelected() {
    const template = this.activeWorkoutPlan?.workoutTemplates.find(t => t.order === this.selectedTemplateOrder);
    setTimeout(() => this.selectedTemplateOrder = null);
    if (!template)
      return;

    if (!this.draft.name.trim()) {
      this.draft.name = template.name ?? '';
    }

    this.draft.copyFromTemplate(template);
    this.toastr.success(`Copied "${template.name || 'workout'}" from your plan`);
    this.router.navigate(['/workout']);
  }
}

function firstOfCurrentMonth(): string {
  const now = new Date();
  const month = String(now.getMonth() + 1).padStart(2, '0');
  return `${now.getFullYear()}-${month}-01`;
}
