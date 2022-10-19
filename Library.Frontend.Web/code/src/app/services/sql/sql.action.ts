export namespace SQLActions {
  export class UpdateConnectionString {
    static readonly type = '[SQL API] Update Connection String';
    constructor(public inputModel: { connectionString: string }) {}
  }

  export class UpdateConnectionStringSuccessful {
    static readonly type = '[SQL API] Update Connection String Successful';
    constructor(public sqlStatus: string) {}
  }

  export class ResetData {
    static readonly type = '[SQL API] Reset Data';
  }

  export class ResetDataSuccessful {
    static readonly type = '[SQL API] Reset Data Successful';
    constructor(public sqlStatus: string) {}
  }

  export class ResetDataFailed {
    static readonly type = '[SQL API] Reset Data Failed';
  }

  export class DeleteAllData {
    static readonly type = '[SQL API] Delete Data';
  }

  export class DeleteAllDataSuccessful {
    static readonly type = '[SQL API] Delete Data Successful';
    constructor(public sqlStatus: string) {}
  }

  export class DeleteAllDataFailed {
    static readonly type = '[SQL API] Delete Data Failed';
  }
}
