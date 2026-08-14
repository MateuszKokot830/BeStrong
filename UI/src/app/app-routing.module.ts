import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { HomeComponent } from './modules/pages/home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: 'search',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/search/search.module').then(m => m.SearchModule)
  },
  {
    path: 'posts',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/posts/posts.module').then(m => m.PostsModule)
  },
  {
    path: 'profile/:username',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/profile/profile.module').then(m => m.ProfileModule)
  },
  {
    path: 'messages',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/messages/messages.module').then(m => m.MessagesModule)
  },
  {
    path: 'workout-plan',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/workout-plan/workout-plan.module').then(m => m.WorkoutPlanModule)
  },
  {
    path: 'workout-plans',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/workout-plans/workout-plans.module').then(m => m.WorkoutPlansModule)
  },
  {
    path: 'statistics',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/statistics/statistics.module').then(m => m.StatisticsModule)
  },
  {
    path: 'workout',
    canActivate: [AuthGuard],
    canLoad: [AuthGuard],
    loadChildren: () => import('./modules/pages/workout/workout.module').then(m => m.WorkoutModule)
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
