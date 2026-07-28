import { MuscleGroup, MuscleSubgroup } from './Enums';

export interface Exercise {
  id: number;
  name: string | null;
  description: string | null;
  muscleGroup: MuscleGroup;
  muscleSubgroup: MuscleSubgroup;
  imageUrl: string | null;
}

export interface ExerciseCreate {
  name: string;
  description: string | null;
  muscleSubgroup: MuscleSubgroup;
  imageUrl: string | null;
}
