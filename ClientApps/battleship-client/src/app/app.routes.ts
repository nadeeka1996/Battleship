import { Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { BattleshipGameComponent } from './battleship-game/battleship-game.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'game/:id', component: BattleshipGameComponent },
];
