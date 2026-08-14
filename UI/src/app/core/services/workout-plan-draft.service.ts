import { Injectable } from '@angular/core';
import { Exercise } from '../models/Exercise';
import { MuscleSubgroup, WorkoutPlanCategory } from '../models/Enums';
import { WorkoutPlan } from '../models/WorkoutPlan';

export interface DraftTemplateExercise {
  exerciseId: number;
  name: string | null;
  imageUrl: string | null;
  muscleSubgroup: MuscleSubgroup;
  sets: number;
  minReps: number;
  maxReps: number;
}

export interface DraftTemplate {
  name: string;
  exercises: DraftTemplateExercise[];
}

@Injectable({
  providedIn: 'root'
})
export class WorkoutPlanDraftService {
  name = '';
  description = '';
  category: WorkoutPlanCategory = WorkoutPlanCategory.FullBody;
  isPublic = false;
  templates: DraftTemplate[] = [this.createTemplate(1)];
  activeTemplateIndex = 0;
  editingPlanId: number | null = null;

  loadFrom(plan: WorkoutPlan) {
    this.editingPlanId = plan.id;
    this.name = plan.name ?? '';
    this.description = plan.description ?? '';
    this.category = plan.category;
    this.isPublic = plan.isPublic;
    this.templates = plan.workoutTemplates.map(t => ({
      name: t.name ?? '',
      exercises: t.exercises.map(e => ({
        exerciseId: e.exercise.id,
        name: e.exercise.name,
        imageUrl: e.exercise.imageUrl,
        muscleSubgroup: e.exercise.muscleSubgroup,
        sets: e.sets,
        minReps: e.minReps,
        maxReps: e.maxReps
      }))
    }));
    this.activeTemplateIndex = 0;
  }

  addTemplate() {
    this.templates.push(this.createTemplate(this.templates.length + 1));
    this.activeTemplateIndex = this.templates.length - 1;
  }

  removeTemplate(index: number) {
    this.templates.splice(index, 1);
    if (this.activeTemplateIndex >= this.templates.length) {
      this.activeTemplateIndex = Math.max(0, this.templates.length - 1);
    }
  }

  setActiveTemplate(index: number) {
    this.activeTemplateIndex = index;
  }

  addExerciseToActiveTemplate(exercise: Exercise) {
    const template = this.templates[this.activeTemplateIndex];
    if (!template)
      return;

    template.exercises.push({
      exerciseId: exercise.id,
      name: exercise.name,
      imageUrl: exercise.imageUrl,
      muscleSubgroup: exercise.muscleSubgroup,
      sets: 3,
      minReps: 8,
      maxReps: 12
    });
  }

  removeExercise(templateIndex: number, exerciseIndex: number) {
    this.templates[templateIndex]?.exercises.splice(exerciseIndex, 1);
  }

  reorderExercise(templateIndex: number, fromIndex: number, toIndex: number) {
    const template = this.templates[templateIndex];
    if (!template || fromIndex === toIndex)
      return;

    const [moved] = template.exercises.splice(fromIndex, 1);
    template.exercises.splice(toIndex, 0, moved);
  }

  isEmpty(): boolean {
    return !this.name.trim()
      && this.templates.length <= 1
      && !this.templates.some(t => t.exercises.length > 0);
  }

  clear() {
    this.name = '';
    this.description = '';
    this.category = WorkoutPlanCategory.FullBody;
    this.isPublic = false;
    this.templates = [this.createTemplate(1)];
    this.activeTemplateIndex = 0;
    this.editingPlanId = null;
  }

  private createTemplate(order: number): DraftTemplate {
    return { name: `Workout ${order}`, exercises: [] };
  }
}
