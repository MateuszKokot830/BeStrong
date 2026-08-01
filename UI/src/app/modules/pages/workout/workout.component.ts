import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Exercise } from 'src/app/core/models/Exercise';
import { MuscleGroup, MuscleSubgroup } from 'src/app/core/models/Enums';
import { WorkoutCreate, WorkoutExercise } from 'src/app/core/models/Workout';
import { AccountService } from 'src/app/core/services/account.service';
import { WorkoutService } from 'src/app/core/services/workout.service';
import { ExerciseComponent } from '../../components/exercise/exercise/exercise.component';

interface ExerciseDraft {
  exerciseId: number;
  sets: number;
  reps: number;
  weight: number;
}

const DEFAULT_EXERCISE_IMAGE = 'assets/photos/defaultPhoto.jpg';

@Component({
    selector: 'app-workout',
    templateUrl: './workout.component.html',
    styleUrls: ['./workout.component.css'],
    standalone: false
})
export class WorkoutComponent implements OnInit {
  exercises: Exercise[] = [];
  exerciseNames = new Map<number, string>();
  workoutName = '';
  drafts: ExerciseDraft[] = [];
  workoutExercise = emptyDraft();
  isSaving = false;
  isAdmin = false;

  muscleGroups = Object.keys(MuscleGroup)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ value: MuscleGroup[key as keyof typeof MuscleGroup], label: key }));

  constructor(private workoutService: WorkoutService, private toastr: ToastrService,
    private modalService: BsModalService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.loadExercises();

    this.accountService.currentProfile().subscribe({
      next: user => this.isAdmin = user.isAdmin
    });
  }

  loadExercises() {
    this.workoutService.getExercises().subscribe({
      next: exercises => {
        this.exercises = exercises;
        this.exerciseNames = new Map(exercises.map(e => [e.id, e.name ?? 'Unknown exercise']));
      }
    });
  }

  getExerciseName(exerciseId: number) {
    return this.exerciseNames.get(exerciseId) ?? 'Unknown exercise';
  }

  exercisesByGroup(group: MuscleGroup | null): Exercise[] {
    return group === null
      ? this.exercises
      : this.exercises.filter(e => e.muscleGroup === group);
  }

  getMuscleSubgroupLabel(exercise: Exercise): string {
    return MuscleSubgroup[exercise.muscleSubgroup] ?? 'Unknown';
  }

  getExerciseImage(exercise: Exercise): string {
    return exercise.imageUrl || DEFAULT_EXERCISE_IMAGE;
  }

  addNewExerciseToggle() {
    const ref = this.modalService.show(ExerciseComponent);
    ref.content?.saved.subscribe(() => this.loadExercises());
  }

  addExercise() {
    const draft = this.workoutExercise;

    if (!draft.exerciseId) {
      this.toastr.error('Pick an exercise first.');
      return;
    }
    if (!draft.sets || !draft.reps) {
      this.toastr.error('Sets and reps are required.');
      return;
    }

    this.drafts.push({ ...draft });
    this.workoutExercise = emptyDraft();
  }

  removeDraft(index: number) {
    this.drafts.splice(index, 1);
  }

  addWorkout() {
    if (!this.drafts.length) {
      this.toastr.error('Add at least one exercise before saving.');
      return;
    }

    const workout: WorkoutCreate = {
      name: this.workoutName,
      exercises: this.drafts.map((draft, index) => this.toWorkoutExercise(draft, index))
    };

    this.isSaving = true;
    this.workoutService.addWorkout(workout).subscribe({
      next: _ => {
        this.isSaving = false;
        this.drafts = [];
        this.workoutName = '';
        this.workoutExercise = emptyDraft();
        this.toastr.success('Workout has been saved!');
      },
      error: _ => this.isSaving = false
    });
  }

  private toWorkoutExercise(draft: ExerciseDraft, index: number): WorkoutExercise {
    const setCount = Number(draft.sets) || 0;

    return {
      order: index + 1,
      notes: null,
      exerciseId: Number(draft.exerciseId),
      workoutId: 0,
      maxTotalWeight: null,
      bestEstimatedOneRepMax: null,
      sets: Array.from({ length: setCount }, (_, i) => ({
        setNumber: i + 1,
        reps: Number(draft.reps) || 0,
        weight: Number(draft.weight) || 0,
        totalWeight: null,
        estimatedOneRepMax: null
      }))
    };
  }
}

function emptyDraft(): ExerciseDraft {
  return { exerciseId: 0, sets: 0, reps: 0, weight: 0 };
}
