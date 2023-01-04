import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Exercise } from 'src/app/core/models/Exercise';
import { UserAuth } from 'src/app/core/models/User';
import { AccountService } from 'src/app/core/services/account.service';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  currentUser: UserAuth | null = null;
  exercises: Exercise[] = [];
  calculatorLift: number;
  calculatorReps: number;

  constructor(private workoutService: WorkoutService, private accountService: AccountService,
    private userService: UserService, private toastr: ToastrService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: currentUser => this.currentUser = currentUser
      });
    }

  ngOnInit(): void {
  }

}
