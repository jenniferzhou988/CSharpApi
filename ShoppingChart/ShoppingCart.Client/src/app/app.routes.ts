import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'products',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/products/product-list/product-list.component').then(m => m.ProductListComponent)
      },
      {
        path: 'new',
        loadComponent: () =>
          import('./features/products/product-form/product-form.component').then(m => m.ProductFormComponent)
      },
      {
        path: 'edit/:id',
        loadComponent: () =>
          import('./features/products/product-form/product-form.component').then(m => m.ProductFormComponent)
      }
    ]
  },
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: '**', redirectTo: '/products' }
];
