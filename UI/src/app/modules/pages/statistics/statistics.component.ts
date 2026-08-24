import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Exercise } from 'src/app/core/models/Exercise';
import { Statistics } from 'src/app/core/models/Statistics';
import { Workout } from 'src/app/core/models/Workout';
import { AccountService } from 'src/app/core/services/account.service';
import { WorkoutService } from 'src/app/core/services/workout.service';

const HISTORY_MONTHS = 3;
const CHART_WIDTH = 560;
const CHART_HEIGHT = 200;
const CHART_PADDING_TOP = 16;
const CHART_PADDING_BOTTOM = 24;
const CHART_PADDING_LEFT = 56;
const CHART_PADDING_RIGHT = 16;

interface ExerciseHistoryPoint {
  date: string;
  maxTotalWeight: number;
  bestEstimatedOneRepMax: number;
}

interface ChartPoint {
  x: number;
  y: number;
  value: number;
  date: string;
}

interface ChartData {
  path: string;
  points: ChartPoint[];
  maxValue: number;
  minValue: number;
}

@Component({
    selector: 'app-statistics',
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.css'],
    standalone: false
})
export class StatisticsComponent implements OnInit {
  statistics: Statistics | null = null;
  calculatorWeight = 0;
  calculatorReps = 0;
  calculatorResult = 0;
  isLoading = false;

  exercises: Exercise[] = [];
  selectedExerciseId: number | null = null;
  exerciseHistory: ExerciseHistoryPoint[] = [];

  readonly chartWidth = CHART_WIDTH;
  readonly chartHeight = CHART_HEIGHT;
  readonly chartTopY = CHART_PADDING_TOP;
  readonly chartBottomY = CHART_HEIGHT - CHART_PADDING_BOTTOM;
  readonly axisLineX = CHART_PADDING_LEFT - 8;
  readonly axisTickX = CHART_PADDING_LEFT - 12;
  readonly axisTitleX = 14;
  readonly axisTitleY = CHART_HEIGHT / 2;

  private allWorkouts: Workout[] = [];

  constructor(private workoutService: WorkoutService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.isLoading = true;

    this.accountService.currentProfile().pipe(
      switchMap(user => forkJoin({
        statistics: this.workoutService.getStatistics(user.id),
        exercises: this.workoutService.getExercises(),
        workouts: this.workoutService.getUserWorkouts(user.id)
      }))
    ).subscribe({
      next: ({ statistics, exercises, workouts }) => {
        this.isLoading = false;
        this.statistics = statistics;
        this.exercises = exercises;
        this.allWorkouts = workouts;

        const favourite = exercises.find(e => e.name === statistics.favouriteExercise);
        this.selectedExerciseId = favourite ? favourite.id : null;
        this.updateExerciseHistory();
      },
      error: _ => this.isLoading = false
    });
  }

  calculate() {
    this.workoutService.calculate(this.calculatorWeight, this.calculatorReps).subscribe({
      next: result => this.calculatorResult = result
    });
  }

  onExerciseSelected() {
    this.updateExerciseHistory();
  }

  get oneRepMaxChart(): ChartData {
    return this.buildChart(
      this.exerciseHistory.map(p => p.bestEstimatedOneRepMax),
      this.exerciseHistory.map(p => p.date)
    );
  }

  get totalWeightChart(): ChartData {
    return this.buildChart(
      this.exerciseHistory.map(p => p.maxTotalWeight),
      this.exerciseHistory.map(p => p.date)
    );
  }

  private updateExerciseHistory() {
    if (this.selectedExerciseId === null) {
      this.exerciseHistory = [];
      return;
    }

    const cutoff = new Date();
    cutoff.setMonth(cutoff.getMonth() - HISTORY_MONTHS);

    this.exerciseHistory = this.allWorkouts
      .filter(w => new Date(w.date) >= cutoff)
      .flatMap(w => {
        const match = w.workoutExercises.find(we => we.exerciseId === this.selectedExerciseId);
        return match ? [{
          date: w.date,
          maxTotalWeight: match.maxTotalWeight ?? 0,
          bestEstimatedOneRepMax: match.bestEstimatedOneRepMax ?? 0
        }] : [];
      })
      .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
  }

  private buildChart(values: number[], dates: string[]): ChartData {
    if (!values.length) {
      return { path: '', points: [], maxValue: 0, minValue: 0 };
    }

    const max = Math.max(...values);
    const min = Math.min(...values);
    const range = max - min || 1;
    const plotWidth = CHART_WIDTH - CHART_PADDING_LEFT - CHART_PADDING_RIGHT;
    const plotHeight = CHART_HEIGHT - CHART_PADDING_TOP - CHART_PADDING_BOTTOM;
    const stepX = values.length > 1 ? plotWidth / (values.length - 1) : 0;

    const points: ChartPoint[] = values.map((value, i) => ({
      x: CHART_PADDING_LEFT + i * stepX,
      y: CHART_HEIGHT - CHART_PADDING_BOTTOM - ((value - min) / range) * plotHeight,
      value,
      date: dates[i]
    }));

    const path = points.map((p, i) => `${i === 0 ? 'M' : 'L'} ${p.x.toFixed(1)} ${p.y.toFixed(1)}`).join(' ');

    return { path, points, maxValue: max, minValue: min };
  }
}
