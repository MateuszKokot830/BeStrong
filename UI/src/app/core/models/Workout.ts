export interface Workout {
  id: number;
  userId: number;
  date: Date;
  name: string;
  workoutExercises: WorkoutExercise[];
}

export interface WorkoutExercise {
  id: number;
  sets: number;
  reps: number;
  weight: number;
  exerciseId: number;
  workoutId: number;
}
