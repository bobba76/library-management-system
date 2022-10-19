export namespace SQLActions {
  export class ResetData {
    static readonly type = '[SQL API] Reset Data';
  }

  export class ResetDataSuccessful {
    static readonly type = '[SQL API] Reset Data Successful';
    constructor(sqlStatus: string) {}
  }

  export class ResetDataFailed {
    static readonly type = '[SQL API] Reset Data Failed';
  }

  export class DeleteAllData {
    static readonly type = '[SQL API] Delete Data';
  }

  export class DeleteAllDataSuccessful {
    static readonly type = '[SQL API] Delete Data Successful';
    constructor(sqlStatus: string) {}
  }

  export class DeleteAllDataFailed {
    static readonly type = '[SQL API] Delete Data Failed';
  }
}
