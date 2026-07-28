import { WorkoutPlanCategory } from './Enums';
import { User } from './User';

export interface WorkoutPlan {
  id: number;
  createdById: number;
  usedBy: User[];
  name: string | null;
  description: string | null;
  category: WorkoutPlanCategory;
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
  exerciseId: number;
}

export interface WorkoutPlanCreate {
  name: string | null;
  description: string | null;
  category: WorkoutPlanCategory;
  isPublic: boolean;
  workoutTemplates: WorkoutTemplate[];
}
