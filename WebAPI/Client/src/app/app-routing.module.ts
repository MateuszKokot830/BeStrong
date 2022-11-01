import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './modules/pages/home/home.component';
import { SearchComponent } from './modules/pages/search/search.component';
import { PostsComponent } from './modules/pages/posts/posts.component';
import { ProfileComponent } from './modules/pages/profile/profile.component';
import { MessagesComponent } from './modules/pages/messages/messages.component';
import { StatisticsComponent } from './modules/pages/statistics/statistics.component';
import { WorkoutComponent } from './modules/pages/workout/workout.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [ 
      {path: 'search', component: SearchComponent},
      {path: 'posts', component: PostsComponent},
      {path: 'user/:id', component: ProfileComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'statistics', component: StatisticsComponent},
      {path: 'workout', component: WorkoutComponent}
    ]
  },
  {path: '**', component: HomeComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
