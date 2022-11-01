import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
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
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from './shared/shared.module';

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
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
