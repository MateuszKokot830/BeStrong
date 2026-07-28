import { Component, OnInit } from '@angular/core';
import { switchMap } from 'rxjs/operators';
import { Statistics } from 'src/app/core/models/Statistics';
import { UserService } from 'src/app/core/services/user.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  statistics: Statistics | null = null;
  calculatorWeight = 0;
  calculatorReps = 0;
  calculatorResult = 0;

  constructor(private workoutService: WorkoutService, private userService: UserService) { }

  ngOnInit(): void {
    this.loadStatistics();
  }

  loadStatistics() {
    this.userService.getCurrentUser().pipe(
      switchMap(user => this.workoutService.getStatistics(user.id))
    ).subscribe({
      next: statistics => this.statistics = statistics
    });
  }

  calculate() {
    this.workoutService.calculate(this.calculatorWeight, this.calculatorReps).subscribe({
      next: result => this.calculatorResult = result
    });
  }
}
