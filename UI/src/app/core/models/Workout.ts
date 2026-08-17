import { PaginationParams } from './Pagination';

export interface Workout {
  id: number;
  userId: number | null;
  date: string;
  name: string | null;
  workoutExercises: WorkoutExercise[];
}

export interface WorkoutExercise {
  order: number;
  notes: string | null;
  exerciseId: number;
  workoutId: number;
  maxTotalWeight: number | null;
  bestEstimatedOneRepMax: number | null;
  sets: WorkoutSet[];
}

export interface WorkoutSet {
  setNumber: number;
  reps: number;
  weight: number | null;
  totalWeight: number | null;
  estimatedOneRepMax: number | null;
}

export interface WorkoutCreate {
  name: string | null;
  exercises: WorkoutExercise[];
}

export interface WorkoutCriteria extends PaginationParams {
  dateFrom?: string;
  dateTo?: string;
  name?: string;
  exerciseId?: number;
}
