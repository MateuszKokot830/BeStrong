import { AfterViewInit, Component, ElementRef, NgZone, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Exercise } from 'src/app/core/models/Exercise';
import { MuscleGroup, MuscleSubgroup } from 'src/app/core/models/Enums';
import { Workout, WorkoutCreate, WorkoutExercise, WorkoutSet } from 'src/app/core/models/Workout';
import { WorkoutPlan } from 'src/app/core/models/WorkoutPlan';
import { AccountService } from 'src/app/core/services/account.service';
import { WorkoutService } from 'src/app/core/services/workout.service';
import { WorkoutPlanService } from 'src/app/core/services/workout-plan.service';
import { DraftExercise, WorkoutDraftService } from 'src/app/core/services/workout-draft.service';
import { ExerciseComponent } from '../../components/exercise/exercise/exercise.component';

const DEFAULT_EXERCISE_IMAGE = 'assets/photos/defaultPhoto.jpg';
const DEFAULT_PANEL_HEIGHT = 600;

@Component({
    selector: 'app-workout',
    templateUrl: './workout.component.html',
    styleUrls: ['./workout.component.css'],
    standalone: false
})
export class WorkoutComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('leftPanel') leftPanelRef!: ElementRef<HTMLElement>;
  @ViewChild('exercisePanel') exercisePanelRef!: ElementRef<HTMLElement>;

  exercises: Exercise[] = [];
  isSaving = false;
  isAdmin = false;
  dragOverIndex: number | null = null;
  rightPanelHeight: number = DEFAULT_PANEL_HEIGHT;
  activeWorkoutPlan: WorkoutPlan | null = null;
  selectedTemplateOrder: number | null = null;

  muscleGroups = Object.keys(MuscleGroup)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ value: MuscleGroup[key as keyof typeof MuscleGroup], label: key }));

  private previousSetsByExercise = new Map<number, WorkoutSet[]>();
  private dragIndex: number | null = null;
  private resizeObserver?: ResizeObserver;
  private currentUserId: number | null = null;

  constructor(private workoutService: WorkoutService, private toastr: ToastrService,
    private modalService: BsModalService, private accountService: AccountService,
    private workoutPlanService: WorkoutPlanService, private zone: NgZone, public draft: WorkoutDraftService) { }

  ngOnInit(): void {
    this.loadExercises();

    this.accountService.currentProfile().subscribe({
      next: user => {
        this.isAdmin = user.isAdmin;
        this.currentUserId = user.id;
        this.refreshPreviousSets();
        this.loadActiveWorkoutPlan(user.workoutPlanId);
      }
    });
  }

  private loadActiveWorkoutPlan(workoutPlanId: number | null): void {
    if (!workoutPlanId) {
      this.activeWorkoutPlan = null;
      return;
    }

    this.workoutPlanService.getWorkoutPlan(workoutPlanId).subscribe({
      next: plan => this.activeWorkoutPlan = plan
    });
  }

  private refreshPreviousSets(): void {
    if (this.currentUserId === null)
      return;

    this.workoutService.getUserWorkouts(this.currentUserId).subscribe({
      next: workouts => this.buildPreviousSetsIndex(workouts)
    });
  }

  ngAfterViewInit(): void {
    this.resizeObserver = new ResizeObserver(() => {
      this.zone.run(() => this.updateRightPanelHeight());
    });
    this.resizeObserver.observe(this.leftPanelRef.nativeElement);
  }

  ngOnDestroy(): void {
    this.resizeObserver?.disconnect();
  }

  loadExercises() {
    this.workoutService.getExercises().subscribe({
      next: exercises => {
        this.exercises = exercises;
        setTimeout(() => this.updateRightPanelHeight());
      }
    });
  }

  onTabSelect(): void {
    setTimeout(() => this.updateRightPanelHeight());
  }

  private updateRightPanelHeight(): void {
    if (!this.leftPanelRef || !this.exercisePanelRef)
      return;

    const leftHeight = this.leftPanelRef.nativeElement.getBoundingClientRect().height;
    const cardEl = this.exercisePanelRef.nativeElement;
    const activeList = cardEl.querySelector<HTMLElement>('tab.active .exercise-list');
    if (!activeList)
      return;

    // Everything in the card besides the scrollable list itself (header, tab nav, padding) —
    // the list is the only flexible piece, so this overhead is constant regardless of the
    // height currently applied.
    const chrome = cardEl.getBoundingClientRect().height - activeList.clientHeight;

    const upperBound = activeList.scrollHeight + chrome;
    const lowerBound = Math.min(DEFAULT_PANEL_HEIGHT, upperBound);

    this.rightPanelHeight = Math.min(Math.max(leftHeight, lowerBound), upperBound);
  }

  exercisesByGroup(group: MuscleGroup | null): Exercise[] {
    return group === null
      ? this.exercises
      : this.exercises.filter(e => e.muscleGroup === group);
  }

  getMuscleSubgroupLabel(exercise: { muscleSubgroup: MuscleSubgroup }): string {
    return MuscleSubgroup[exercise.muscleSubgroup] ?? 'Unknown';
  }

  getExerciseImage(exercise: { imageUrl: string | null }): string {
    return exercise.imageUrl || DEFAULT_EXERCISE_IMAGE;
  }

  addNewExerciseToggle() {
    const ref = this.modalService.show(ExerciseComponent);
    ref.content?.saved.subscribe(() => this.loadExercises());
  }

  addExerciseToWorkout(exercise: Exercise) {
    this.draft.addExercise(exercise);
    this.toastr.success(`${exercise.name} added to workout`);
  }

  removeExerciseFromWorkout(index: number) {
    this.draft.removeExercise(index);
  }

  addSet(exerciseIndex: number) {
    this.draft.addSet(exerciseIndex);
  }

  removeSet(exerciseIndex: number, setIndex: number) {
    this.draft.removeSet(exerciseIndex, setIndex);
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
  }

  cancelWorkout() {
    if (this.draft.isEmpty())
      return;

    if (confirm('This will discard the workout you are building. Continue?')) {
      this.draft.clear();
    }
  }

  onDragStart(index: number) {
    this.dragIndex = index;
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDrop(index: number) {
    if (this.dragIndex !== null) {
      this.draft.reorder(this.dragIndex, index);
    }
    this.dragIndex = null;
    this.dragOverIndex = null;
  }

  onDragEnd() {
    this.dragIndex = null;
    this.dragOverIndex = null;
  }

  getPreviousLabel(exerciseId: number, setIndex: number): string {
    const setNumber = setIndex + 1;
    const set = this.previousSetsByExercise.get(exerciseId)?.find(s => s.setNumber === setNumber);
    return set ? `${set.weight ?? 0}kg x ${set.reps}` : '-';
  }

  addWorkout() {
    if (!this.draft.exercises.length) {
      this.toastr.error('Add at least one exercise before saving.');
      return;
    }

    const hasMissingReps = this.draft.exercises.some(e => e.sets.some(s => !s.reps));
    if (hasMissingReps) {
      this.toastr.error('Fill in reps for every set.');
      return;
    }

    const workout: WorkoutCreate = {
      name: this.draft.name || null,
      exercises: this.draft.exercises.map((exercise, index) => this.toWorkoutExercise(exercise, index))
    };

    this.isSaving = true;
    this.workoutService.addWorkout(workout).subscribe({
      next: _ => {
        this.isSaving = false;
        this.draft.clear();
        this.refreshPreviousSets();
        this.toastr.success('Workout has been saved!');
      },
      error: _ => this.isSaving = false
    });
  }

  private toWorkoutExercise(exercise: DraftExercise, index: number): WorkoutExercise {
    return {
      order: index + 1,
      notes: exercise.notes,
      exerciseId: exercise.exerciseId,
      workoutId: 0,
      maxTotalWeight: null,
      bestEstimatedOneRepMax: null,
      sets: exercise.sets.map((set, setIndex) => ({
        setNumber: setIndex + 1,
        reps: Number(set.reps) || 0,
        weight: set.weight !== null ? Number(set.weight) : null,
        totalWeight: null,
        estimatedOneRepMax: null
      }))
    };
  }

  private buildPreviousSetsIndex(workouts: Workout[]) {
    const map = new Map<number, WorkoutSet[]>();

    for (const workout of workouts) {
      for (const workoutExercise of workout.workoutExercises) {
        if (!map.has(workoutExercise.exerciseId)) {
          map.set(workoutExercise.exerciseId, workoutExercise.sets);
        }
      }
    }

    this.previousSetsByExercise = map;
  }
}
