import {
  CreateLibraryItemInputModel,
  LibraryItemModel,
  UpdateLibraryItemInputModel,
} from '@models/library-item/library-item.model';

export namespace LibraryItemActions {
  export class ClearStore {
    static readonly type = '[LibraryItem] Clear Store';
  }

  export class Get {
    static readonly type = '[LibraryItem API] Get All';
  }

  export class GetById {
    static readonly type = '[LibraryItem API] Get by ID';
    constructor(public id: number) {}
  }

  export class Create {
    static readonly type = '[LibraryItem API] Create';
    constructor(public inputModel: CreateLibraryItemInputModel) {}
  }

  export class CreateSuccessful {
    static readonly type = '[LibraryItem API] Create Successful';
    constructor(public libraryItems: Array<LibraryItemModel>) {}
  }

  export class CreateFailed {
    static readonly type = '[LibraryItem API] Create Failed';
  }

  export class Update {
    static readonly type = '[LibraryItem API] Update';
    constructor(
      public id: number,
      public inputModel: UpdateLibraryItemInputModel
    ) {}
  }

  export class UpdateSuccessful {
    static readonly type = '[LibraryItem API] Update Successful';
    constructor(public libraryItem: LibraryItemModel) {}
  }

  export class UpdateFailed {
    static readonly type = '[LibraryItem API] Update Failed';
  }

  export class Delete {
    static readonly type = '[LibraryItem API] Delete';
    constructor(public id: number) {}
  }

  export class DeleteSuccessful {
    static readonly type = '[LibraryItem API] Delete Successful';
    constructor(public libraryItems: Array<LibraryItemModel>) {}
  }

  export class DeleteFailed {
    static readonly type = '[LibraryItem API] Delete Failed';
  }
}
