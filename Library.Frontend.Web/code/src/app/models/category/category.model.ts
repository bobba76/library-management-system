import { Entity } from '@shared/models/entity.interface';

class CategoryModel implements Entity {
  id: number;
  categoryName: string;

  constructor() {
    this.id = 0;
    this.categoryName = '';
  }
}

type CreateCategoryInputModel = {
  categoryName: string;
};

type UpdateCategoryInputModel = {
  categoryName: string;
};

export { CategoryModel, CreateCategoryInputModel, UpdateCategoryInputModel };
