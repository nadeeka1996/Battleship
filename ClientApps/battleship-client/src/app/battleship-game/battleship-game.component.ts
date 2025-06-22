import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { BattleshipApiService } from './battleship-api.service';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-battleship-game',
  standalone: true,
  imports: [CommonModule, MatCardModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,],
  templateUrl: './battleship-game.component.html',
  styleUrls: ['./battleship-game.component.scss']
})
export class BattleshipGameComponent {
  gridSize = 10;
  gameId: string | null = null;
  lastGameId: string | null = null;
  grid: { row: number, col: number, state: 'empty' | 'hit' | 'miss' | 'sunk' }[][] = [];
  isStarted = false;
  isLoading = false;
  winMessage: string | null = null;

  constructor(private api: BattleshipApiService, private route: ActivatedRoute) {
    this.initGrid();
    this.route.paramMap.subscribe(params => {
      this.gameId = params.get('id');
    });
  }

  ngOnInit() {}
  initGrid() {
    this.grid = Array.from({ length: this.gridSize }, (_, row) =>
      Array.from({ length: this.gridSize }, (_, col) => ({ row, col, state: 'empty' }))
    );
  }

  cellClick(row: number, col: number) {
    if (!this.gameId || this.grid[row][col].state !== 'empty' || this.winMessage) return;
    this.isLoading = true;
    const coordinate = String.fromCharCode(65 + col) + (row + 1); 
    this.api.attack(this.gameId, coordinate).subscribe(res => {
      this.grid[row][col].state = res.hit ? 'hit' : 'miss';
      this.isLoading = false;
      this.api.getGameState(this.gameId!).subscribe(state => {
        if (Array.isArray(state.ships)) {
          state.ships.forEach((ship: any) => {
            if (ship.isSunk && Array.isArray(ship.positions)) {
              ship.positions.forEach((pos: any) => {
                const colIdx = pos.column.charCodeAt(0) - 65;
                const rowIdx = pos.row - 1;
                if (this.grid[rowIdx] && this.grid[rowIdx][colIdx]) {
                  this.grid[rowIdx][colIdx].state = 'sunk';
                }
              });
            }
          });
        }
         const allSunk = Array.isArray(state.ships) && state.ships.length === 3 && state.ships.every((ship: any) => ship.isSunk);
        if (allSunk) {
          this.winMessage = 'Congratulations! You win!';
          this.lastGameId = this.gameId;
          this.gameId = null; 
        }
      });
    }, () => {
      this.isLoading = false;
    });
  }

  playAgain() {
    this.isLoading = true;
    this.api.startGame().subscribe((res: any) => {
      this.isLoading = false;
      window.location.href = '/game/' + res.id;
    }, () => {
      this.isLoading = false;
    });
  }

  getRowLabel(row: number): string {
    return (row + 1).toString();
  }

  getColLabel(col: number): string {
    return String.fromCharCode(65 + col);
  }

  getCellColor(row: number, col: number): string {
    return '#f0f0f0';
  }
}
