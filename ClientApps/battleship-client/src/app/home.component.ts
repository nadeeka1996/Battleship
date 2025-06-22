import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BattleshipApiService } from './battleship-game/battleship-api.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatCardModule,
    MatButtonModule,MatIconModule],
  template: `
    <main style="display: flex; justify-content: center; align-items: center; height: 100vh;">
  <mat-card style="padding: 32px; text-align: center; max-width: 400px; width: 100%;">
    <h1 style="margin-bottom: 24px;">
      <mat-icon style="vertical-align: middle;  margin-right: 8px;">directions_boat</mat-icon>
      Battleship Game
    </h1>
    <button mat-raised-button color="primary" (click)="startGame()" [disabled]="isLoading">
      {{ isLoading ? 'Starting...' : 'Start Game' }}
    </button>
  </mat-card>
</main>

  `
})
export class HomeComponent {
  isLoading = false;
  constructor(private api: BattleshipApiService, private router: Router) {}
  startGame() {
    this.isLoading = true;
    this.api.startGame().subscribe((res: any) => {
      this.isLoading = false;
      this.router.navigate(['/game', res.id]);
    }, () => {
      this.isLoading = false;
    });
  }
}
