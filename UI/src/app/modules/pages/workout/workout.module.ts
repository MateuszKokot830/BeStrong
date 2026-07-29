import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { ExerciseComponent } from '../../components/exercise/exercise/exercise.component';
import { WorkoutComponent } from './workout.component';

@NgModule({
  declarations: [
    WorkoutComponent,
    ExerciseComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([{ path: '', component: WorkoutComponent }])
  ]
})
export class WorkoutModule { }
