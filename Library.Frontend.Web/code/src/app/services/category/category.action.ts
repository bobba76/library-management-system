import {
  CategoryModel, CreateCategoryInputModel, UpdateCategoryInputModel
} from '@models/category/category.model';

export namespace CategoryActions {
  export class ClearStore {
    static readonly type = '[Category] Clear Store';
  }

  export class Get {
    static readonly type = '[Category API] Get All';
  }

  export class GetById {
    static readonly type = '[Category API] Get by ID';
    constructor(public id: number) {}
  }

  export class Create {
    static readonly type = '[Category API] Create';
    constructor(public inputModel: CreateCategoryInputModel) {}
  }

  export class CreateSuccessful {
    static readonly type = '[Category API] Create Successful';
    constructor(public categories: Array<CategoryModel>) {}
  }

  export class CreateFailed {
    static readonly type = '[Category API] Create Failed';
  }

  export class Update {
    static readonly type = '[Category API] Update';
    constructor(
      public id: number,
      public inputModel: UpdateCategoryInputModel
    ) {}
  }

  export class UpdateSuccessful {
    static readonly type = '[Category API] Update Successful';
    constructor(public category: CategoryModel) {}
  }

  export class UpdateFailed {
    static readonly type = '[Category API] Update Failed';
  }

  export class Delete {
    static readonly type = '[Category API] Delete';
    constructor(public id: number) {}
  }

  export class DeleteSuccessful {
    static readonly type = '[Category API] Delete Successful';
    constructor(public categories: Array<CategoryModel>) {}
  }

  export class DeleteFailed {
    static readonly type = '[Category API] Delete Failed';
  }
}
