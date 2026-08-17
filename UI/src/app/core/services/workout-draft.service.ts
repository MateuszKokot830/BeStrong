import { Injectable } from '@angular/core';
import { Exercise } from '../models/Exercise';
import { MuscleSubgroup } from '../models/Enums';
import { WorkoutTemplate } from '../models/WorkoutPlan';

export interface DraftSet {
  reps: number | null;
  weight: number | null;
}

export interface DraftExercise {
  exerciseId: number;
  name: string | null;
  imageUrl: string | null;
  muscleSubgroup: MuscleSubgroup;
  notes: string | null;
  sets: DraftSet[];
}

@Injectable({
  providedIn: 'root'
})
export class WorkoutDraftService {
  name = '';
  exercises: DraftExercise[] = [];

  addExercise(exercise: Exercise) {
    this.exercises.push({
      exerciseId: exercise.id,
      name: exercise.name,
      imageUrl: exercise.imageUrl,
      muscleSubgroup: exercise.muscleSubgroup,
      notes: null,
      sets: [{ reps: null, weight: null }]
    });
  }

  removeExercise(index: number) {
    this.exercises.splice(index, 1);
  }

  copyFromWorkout(name: string | null, exercises: DraftExercise[]) {
    this.name = name ?? '';
    this.exercises = exercises;
  }

  copyFromTemplate(template: WorkoutTemplate) {
    for (const templateExercise of template.exercises) {
      this.exercises.push({
        exerciseId: templateExercise.exercise.id,
        name: templateExercise.exercise.name,
        imageUrl: templateExercise.exercise.imageUrl,
        muscleSubgroup: templateExercise.exercise.muscleSubgroup,
        notes: null,
        sets: Array.from({ length: templateExercise.sets }, () => ({ reps: templateExercise.minReps, weight: null }))
      });
    }
  }

  addSet(exerciseIndex: number) {
    this.exercises[exerciseIndex]?.sets.push({ reps: null, weight: null });
  }

  removeSet(exerciseIndex: number, setIndex: number) {
    this.exercises[exerciseIndex]?.sets.splice(setIndex, 1);
  }

  reorder(fromIndex: number, toIndex: number) {
    if (fromIndex === toIndex)
      return;

    const [moved] = this.exercises.splice(fromIndex, 1);
    this.exercises.splice(toIndex, 0, moved);
  }

  isEmpty(): boolean {
    return !this.name.trim() && this.exercises.length === 0;
  }

  clear() {
    this.name = '';
    this.exercises = [];
  }
}
