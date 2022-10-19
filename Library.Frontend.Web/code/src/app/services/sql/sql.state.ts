import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';

import {
  EmployeeModel,
  EmployeeRole,
  employeeRoleName
} from '@models/employee/employee.model';
import { first, tap } from 'rxjs';
import { SQLActions } from './sql.action';
import { SQLService } from './sql.service';

export interface SQLStateModel {
  sqlStatus: string;
}

@State<SQLStateModel>({
  name: 'sql',
  defaults: {
    sqlStatus: '',
  },
})
@Injectable()
export class SQLState {
  constructor(private sqlService: SQLService) {}

  /* ---------- Selectors ---------- */
  @Selector([SQLState])
  static getStatus(state: SQLStateModel) {
    return state.sqlStatus;
  }

  /* ---------- Actions ---------- */
  @Action(SQLActions.ResetData)
  ResetData(ctx: StateContext<SQLStateModel>) {
    return this.sqlService
      .resetData()
      .pipe(first())
      .subscribe({
        next: (sqlStatus) =>
          ctx.dispatch(new SQLActions.ResetDataSuccessful(sqlStatus)),
        error: () => ctx.dispatch(new SQLActions.ResetDataFailed()),
      });
  }

  @Action(SQLActions.ResetDataSuccessful)
  ResetDataSuccessful(ctx: StateContext<SQLStateModel>, sqlStatus: string) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      sqlStatus,
    });
  }

  @Action(SQLActions.DeleteAllData)
  DeleteAllData(ctx: StateContext<SQLStateModel>) {
    return this.sqlService
      .deleteAllData()
      .pipe(first())
      .subscribe({
        next: (sqlStatus) =>
          ctx.dispatch(new SQLActions.DeleteAllDataSuccessful(sqlStatus)),
        error: () => ctx.dispatch(new SQLActions.DeleteAllDataFailed()),
      });
  }

  @Action(SQLActions.DeleteAllDataSuccessful)
  DeleteAllDataSuccessful(ctx: StateContext<SQLStateModel>, sqlStatus: string) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      sqlStatus,
    });
  }
}
