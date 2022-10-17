import {
  CreateEmployeeInputModel,
  UpdateEmployeeInputModel
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

  export class Update {
    static readonly type = '[Employee API] Update';
    constructor(
      public id: number,
      public inputModel: UpdateEmployeeInputModel
    ) {}
  }

  export class Delete {
    static readonly type = '[Employee API] Delete';
    constructor(public id: number) {}
  }
}
