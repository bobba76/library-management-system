import {
  CreateEmployeeInputModel,
  EmployeeModel,
  UpdateEmployeeInputModel,
} from '@models/employee/employee.model';

export namespace EmployeeActions {
  export class ClearStore {
    static readonly type = '[Employee] Clear Store';
  }

  export class Get {
    static readonly type = '[Employee API] Get All';
  }

  export class GetById {
    static readonly type = '[Employee API] Get by ID';
    constructor(public id: number) {}
  }

  export class Create {
    static readonly type = '[Employee API] Create';
    constructor(public inputModel: CreateEmployeeInputModel) {}
  }

  export class CreateSuccessful {
    static readonly type = '[Employee API] Create Successful';
    constructor(public employees: Array<EmployeeModel>) {}
  }

  export class CreateFailed {
    static readonly type = '[Employee API] Create Failed';
  }

  export class Update {
    static readonly type = '[Employee API] Update';
    constructor(
      public id: number,
      public inputModel: UpdateEmployeeInputModel
    ) {}
  }

  export class UpdateSuccessful {
    static readonly type = '[Employee API] Update Successful';
    constructor(public employee: EmployeeModel) {}
  }

  export class UpdateFailed {
    static readonly type = '[Employee API] Update Failed';
  }

  export class Delete {
    static readonly type = '[Employee API] Delete';
    constructor(public id: number) {}
  }

  export class DeleteSuccessful {
    static readonly type = '[Employee API] Delete Successful';
    constructor(public employees: Array<EmployeeModel>) {}
  }

  export class DeleteFailed {
    static readonly type = '[Employee API] Delete Failed';
  }
}
