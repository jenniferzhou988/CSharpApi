import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { AuthResponse, AuthState, LoginRequest } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private authState$ = new BehaviorSubject<AuthState | null>(this.loadState());

  /** Observable stream of the current auth state. */
  readonly currentUser$ = this.authState$.asObservable();

  constructor(private http: HttpClient) {}

  get isLoggedIn(): boolean {
    return !!this.authState$.value?.accessToken;
  }

  get currentUser(): AuthState | null {
    return this.authState$.value;
  }

  get roles(): string[] {
    return this.authState$.value?.roles ?? [];
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(API_ENDPOINTS.auth.login, request).pipe(
      tap(response => this.saveState(response))
    );
  }

  logout(): void {
    const state = this.authState$.value;
    if (state?.refreshToken) {
      this.http.post(API_ENDPOINTS.auth.logout, { refreshToken: state.refreshToken }).subscribe();
    }
    this.clearState();
  }

  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.authState$.value?.refreshToken;
    return this.http.post<AuthResponse>(API_ENDPOINTS.auth.refresh, { refreshToken }).pipe(
      tap(response => this.saveState(response))
    );
  }

  private saveState(response: AuthResponse): void {
    const roles = this.parseRolesFromJwt(response.accessToken);
    const state: AuthState = {
      userId: response.userId,
      email: response.email,
      accessToken: response.accessToken,
      refreshToken: response.refreshToken,
      expires: response.expires,
      roles
    };
    localStorage.setItem(environment.tokenKey, response.accessToken);
    localStorage.setItem(environment.refreshTokenKey, response.refreshToken);
    localStorage.setItem('auth_state', JSON.stringify(state));
    this.authState$.next(state);
  }

  private loadState(): AuthState | null {
    try {
      const raw = localStorage.getItem('auth_state');
      return raw ? JSON.parse(raw) as AuthState : null;
    } catch {
      return null;
    }
  }

  private clearState(): void {
    localStorage.removeItem(environment.tokenKey);
    localStorage.removeItem(environment.refreshTokenKey);
    localStorage.removeItem('auth_state');
    this.authState$.next(null);
  }

  private parseRolesFromJwt(token: string): string[] {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const roleClaim =
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ??
        payload['role'] ??
        [];
      return Array.isArray(roleClaim) ? roleClaim : [roleClaim];
    } catch {
      return [];
    }
  }
}
