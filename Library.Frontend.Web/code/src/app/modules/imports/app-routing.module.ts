import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () =>
      import('../../components/home/home.module').then((m) => m.HomeModule),
  },
  // {
  //   path: '',
  //   loadChildren: () =>
  //     import('./components/library/library.module').then(
  //       (m) => m.LibraryModule
  //     ),
  // },
  {
    path: '',
    loadChildren: () =>
      import('../../components/employee/employee.module').then(
        (m) => m.EmployeeModule
      ),
  },
  {
    path: '**',
    redirectTo: '',
  },
];

export const routerModule = [RouterModule.forRoot(routes)];
