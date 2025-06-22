import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BattleshipApiService {
  private baseUrl = environment.apiBaseUrl + '/games';

  constructor(private http: HttpClient) {}

  startGame(): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`${this.baseUrl}`, {});
  }

  getGameState(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${id}`);
  }

  attack(
    id: string,
    coordinate: string
  ): Observable<{ hit: boolean; sunk?: boolean; victory?: boolean }> {
    return this.http.post<{ hit: boolean; sunk?: boolean; victory?: boolean }>(
      `${this.baseUrl}/${id}/shots`,
      JSON.stringify({ coordinate }),
      {
        headers: { 'Content-Type': 'application/json' },
      }
    );
  }
}
