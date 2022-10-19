import { RouterModule, Routes } from '@angular/router';

// Lazy loading modules
const routes: Routes = [
  { path: '', redirectTo: 'library', pathMatch: 'full' },
  {
    path: 'library',
    loadChildren: () =>
      import('../../components/library/library.module').then(
        (m) => m.LibraryModule
      ),
  },
  {
    path: 'employee',
    loadChildren: () =>
      import('../../components/employee/employee.module').then(
        (m) => m.EmployeeModule
      ),
  },
  {
    path: '**',
    redirectTo: 'library',
  },
];

export const routerModule = [RouterModule.forRoot(routes)];
