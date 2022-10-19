import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, ActionType, ofActionSuccessful, Store } from '@ngxs/store';

import { SQLActions } from '@services/sql/sql.action';

@Component({
  selector: 'library-menu-bar',
  templateUrl: './menu-bar.component.html',
  styleUrls: ['./menu-bar.component.scss'],
})
export class MenuBarComponent {
  constructor(
    private store: Store,
    private router: Router,
    private actions$: Actions
  ) {}

  deleteAllData(): void {
    this.store.dispatch(new SQLActions.DeleteAllData());

    // Refresh the application.
    this.refreshOnSuccess(SQLActions.DeleteAllDataSuccessful);
  }

  resetData(): void {
    this.store.dispatch(new SQLActions.ResetData());

    // Refresh the application.
    this.refreshOnSuccess(SQLActions.ResetDataSuccessful);
  }

  refreshOnSuccess(action: ActionType): void {
    this.actions$.pipe(ofActionSuccessful(action)).subscribe(() => {
      this.redirectTo(this.router.url);
    });
  }

  redirectTo(uri: string) {
    this.router
      .navigateByUrl('/', { skipLocationChange: true })
      .then(() => this.router.navigate([uri]));
  }
}
