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
  constructor(private store: Store, private actions$: Actions) {}

  updateConnectionString(): void {
    let connectionString = prompt('Ange connection-string:');

    if (connectionString) {
      const inputModel: { connectionString: string } = {
        connectionString,
      };

      this.store.dispatch(new SQLActions.UpdateConnectionString(inputModel));

      // Refresh the application.
      this.refreshOnSuccess(SQLActions.UpdateConnectionStringSuccessful);
    } else {
      alert('Ändringen avbröts.');
    }
  }

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
      this.refresh();
    });
  }

  refresh() {
    location.reload();
  }
}
