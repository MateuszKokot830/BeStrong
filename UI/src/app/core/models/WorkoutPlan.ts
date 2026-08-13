import { Exercise } from './Exercise';
import { WorkoutPlanCategory } from './Enums';
import { User } from './User';

export interface WorkoutPlan {
  id: number;
  createdById: number;
  usedBy: User[];
  name: string | null;
  description: string | null;
  category: WorkoutPlanCategory;
  categoryLabel: string;
  isPublic: boolean;
  workoutTemplates: WorkoutTemplate[];
}

export interface WorkoutTemplate {
  order: number;
  name: string | null;
  exercises: WorkoutTemplateExercise[];
}

export interface WorkoutTemplateExercise {
  order: number;
  exercise: Exercise;
  sets: number;
  minReps: number;
  maxReps: number;
}

export interface WorkoutPlanCreate {
  name: string | null;
  description: string | null;
  category: WorkoutPlanCategory;
  isPublic: boolean;
  workoutTemplates: WorkoutTemplateCreate[];
}

export interface WorkoutTemplateCreate {
  order: number;
  name: string | null;
  exercises: WorkoutTemplateExerciseCreate[];
}

export interface WorkoutTemplateExerciseCreate {
  order: number;
  exerciseId: number;
  sets: number;
  minReps: number;
  maxReps: number;
}

export interface WorkoutPlanCategoryOption {
  value: WorkoutPlanCategory;
  name: string;
  label: string;
}
