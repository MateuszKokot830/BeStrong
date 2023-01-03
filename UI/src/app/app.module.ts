import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './modules/pages/home/home.component';
import { RegisterComponent } from './modules/components/register/register.component';
import { ProfileComponent } from './modules/pages/profile/profile.component';
import { PostsComponent } from './modules/pages/posts/posts.component';
import { MessagesComponent } from './modules/pages/messages/messages.component';
import { StatisticsComponent } from './modules/pages/statistics/statistics.component';
import { WorkoutComponent } from './modules/pages/workout/workout.component';
import { SearchComponent } from './modules/pages/search/search.component';
import { SharedModule } from './shared/shared.module';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { UserCardComponent } from './shared/components/user-card/user-card.component';
import { ExerciseComponent } from './modules/components/exercise/exercise/exercise.component';
import { AddPostComponent } from './modules/components/add-post/add-post.component';
import { AddCommentComponent } from './modules/components/add-comment/add-comment.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    RegisterComponent,
    ProfileComponent,
    PostsComponent,
    MessagesComponent,
    StatisticsComponent,
    WorkoutComponent,
    SearchComponent,
    UserCardComponent,
    ExerciseComponent,
    AddPostComponent,
    AddCommentComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
