import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Statistics } from 'src/app/core/models/Statistics';
import { User, UserAuth } from 'src/app/core/models/User';
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
  statistics = {} as Statistics;
  calculatorWeight: number;
  calculatorReps: number;
  calculatorResult: number;

  constructor(private workoutService: WorkoutService, private accountService: AccountService,
    private userService: UserService, private toastr: ToastrService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: currentUser => this.currentUser = currentUser
      });
    }

  ngOnInit(): void {
    this.loadStatistics();
    this.calculatorWeight = 0;
    this.calculatorReps = 0;
    this.calculatorResult = 0;
  }

  loadStatistics() {
    this.userService.getUser(this.currentUser.username).subscribe({
      next: user=> {
          this.workoutService.getStatistics(user).subscribe({
          next: statistics => this.statistics = statistics
        })
      }
    });
  }

  calculate() {
    this.workoutService.calculate(this.calculatorWeight, this.calculatorReps).subscribe({
      next: result => this.calculatorResult = result
    })
  }

}
