import { AfterViewInit, Component, ElementRef, NgZone, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Exercise } from 'src/app/core/models/Exercise';
import { MuscleGroup, MuscleSubgroup } from 'src/app/core/models/Enums';
import { WorkoutPlanCategoryOption, WorkoutPlanCreate, WorkoutTemplateCreate } from 'src/app/core/models/WorkoutPlan';
import { WorkoutService } from 'src/app/core/services/workout.service';
import { WorkoutPlanService } from 'src/app/core/services/workout-plan.service';
import { DraftTemplate, WorkoutPlanDraftService } from 'src/app/core/services/workout-plan-draft.service';

const DEFAULT_EXERCISE_IMAGE = 'assets/photos/defaultPhoto.jpg';
const DEFAULT_PANEL_HEIGHT = 600;

@Component({
    selector: 'app-workout-plan',
    templateUrl: './workout-plan.component.html',
    styleUrls: ['./workout-plan.component.css'],
    standalone: false
})
export class WorkoutPlanComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('leftPanel') leftPanelRef!: ElementRef<HTMLElement>;
  @ViewChild('exercisePanel') exercisePanelRef!: ElementRef<HTMLElement>;

  exercises: Exercise[] = [];
  isSaving = false;
  dragOverTemplateIndex: number | null = null;
  dragOverIndex: number | null = null;
  rightPanelHeight: number = DEFAULT_PANEL_HEIGHT;

  muscleGroups = Object.keys(MuscleGroup)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ value: MuscleGroup[key as keyof typeof MuscleGroup], label: key }));

  categories: WorkoutPlanCategoryOption[] = [];

  private dragTemplateIndex: number | null = null;
  private dragIndex: number | null = null;
  private resizeObserver?: ResizeObserver;

  constructor(private workoutService: WorkoutService, private workoutPlanService: WorkoutPlanService,
    private toastr: ToastrService, private zone: NgZone, public draft: WorkoutPlanDraftService,
    private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.loadExercises();
    this.loadCategories();

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.workoutPlanService.getWorkoutPlan(+id).subscribe({
          next: plan => this.draft.loadFrom(plan),
          error: _ => this.router.navigateByUrl('/workout-plans')
        });
      } else {
        this.draft.editingPlanId = null;
      }
    });
  }

  loadCategories() {
    this.workoutPlanService.getCategories().subscribe({
      next: categories => this.categories = categories
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

  activeTemplateName(): string {
    return this.draft.templates[this.draft.activeTemplateIndex]?.name || 'workout';
  }

  addTemplate() {
    this.draft.addTemplate();
  }

  removeTemplate(index: number) {
    if (this.draft.templates.length === 1) {
      this.toastr.error('A workout plan needs at least one workout.');
      return;
    }

    if (confirm('Remove this workout from the plan?')) {
      this.draft.removeTemplate(index);
    }
  }

  selectTemplate(index: number) {
    this.draft.setActiveTemplate(index);
  }

  addExerciseToActiveTemplate(exercise: Exercise) {
    this.draft.addExerciseToActiveTemplate(exercise);
    this.toastr.success(`${exercise.name} added to ${this.activeTemplateName()}`);
  }

  removeExerciseFromTemplate(templateIndex: number, exerciseIndex: number) {
    this.draft.removeExercise(templateIndex, exerciseIndex);
  }

  onDragStart(templateIndex: number, exerciseIndex: number) {
    this.dragTemplateIndex = templateIndex;
    this.dragIndex = exerciseIndex;
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDrop(templateIndex: number, exerciseIndex: number) {
    if (this.dragTemplateIndex === templateIndex && this.dragIndex !== null) {
      this.draft.reorderExercise(templateIndex, this.dragIndex, exerciseIndex);
    }
    this.dragTemplateIndex = null;
    this.dragIndex = null;
    this.dragOverTemplateIndex = null;
    this.dragOverIndex = null;
  }

  onDragEnd() {
    this.dragTemplateIndex = null;
    this.dragIndex = null;
    this.dragOverTemplateIndex = null;
    this.dragOverIndex = null;
  }

  returnToPlans() {
    this.router.navigateByUrl('/workout-plans');
  }

  cancelPlan() {
    if (this.draft.isEmpty())
      return;

    const isEditing = this.draft.editingPlanId !== null;
    const message = isEditing
      ? 'Discard your changes to this workout plan? Any unsaved edits will be lost.'
      : 'This will discard the workout plan you are building. Continue?';

    if (confirm(message)) {
      this.draft.clear();
      if (isEditing) {
        this.router.navigateByUrl('/workout-plans');
      }
    }
  }

  savePlan() {
    if (!this.draft.name.trim()) {
      this.toastr.error('Give your workout plan a name.');
      return;
    }

    const emptyTemplate = this.draft.templates.some(t => t.exercises.length === 0);
    if (emptyTemplate) {
      this.toastr.error('Every workout in the plan needs at least one exercise.');
      return;
    }

    const allExercises = this.draft.templates.flatMap(t => t.exercises);
    const hasInvalidSets = allExercises.some(e => !e.sets || e.sets < 1);
    if (hasInvalidSets) {
      this.toastr.error('Every exercise needs at least 1 set.');
      return;
    }

    const hasInvalidReps = allExercises.some(e => !e.minReps || e.minReps < 1 || !e.maxReps || e.maxReps < e.minReps);
    if (hasInvalidReps) {
      this.toastr.error('Every exercise needs a valid rep range (max reps cannot be less than min reps).');
      return;
    }

    const plan: WorkoutPlanCreate = {
      name: this.draft.name,
      description: this.draft.description || null,
      category: this.draft.category,
      isPublic: this.draft.isPublic,
      workoutTemplates: this.draft.templates.map((template, index) => this.toWorkoutTemplate(template, index))
    };

    const editingPlanId = this.draft.editingPlanId;
    const request = editingPlanId
      ? this.workoutPlanService.updateWorkoutPlan(editingPlanId, plan)
      : this.workoutPlanService.addWorkoutPlan(plan);

    this.isSaving = true;
    request.subscribe({
      next: _ => {
        this.isSaving = false;
        this.draft.clear();
        this.toastr.success('Workout plan has been saved!');
        if (editingPlanId) {
          this.router.navigateByUrl('/workout-plans');
        }
      },
      error: _ => this.isSaving = false
    });
  }

  private toWorkoutTemplate(template: DraftTemplate, index: number): WorkoutTemplateCreate {
    return {
      order: index + 1,
      name: template.name || null,
      exercises: template.exercises.map((exercise, exerciseIndex) => ({
        order: exerciseIndex + 1,
        exerciseId: exercise.exerciseId,
        sets: Number(exercise.sets),
        minReps: Number(exercise.minReps),
        maxReps: Number(exercise.maxReps)
      }))
    };
  }
}
