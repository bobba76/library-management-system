import { Entity } from '@shared/models/entity.interface';

class LibraryItemModel implements Entity {
  id: number;
  categoryId: number;
  title: string;
  author?: string;
  pages?: number;
  runTimeMinutes?: number;
  isBorrowable: boolean;
  borrower?: string;
  borrowDate?: string;
  type: LibraryItemType;

  constructor() {
    this.id = 0;
    this.categoryId = 0;
    this.title = '';
    this.author = '';
    this.pages = null;
    this.runTimeMinutes = null;
    this.isBorrowable = true;
    this.borrower = null;
    this.borrowDate = null;
    this.type = LibraryItemType.Book;
  }
}

enum LibraryItemType {
  Book = 1,
  Dvd = 2,
  AudioBook = 3,
  ReferenceBook = 4,
}

// [GET NAME] => libraryItemTypeName.get(LibraryItemType.Employee)
const libraryItemTypeName = new Map<LibraryItemType, string>([
  [LibraryItemType.Book, 'Bok'],
  [LibraryItemType.Dvd, 'DVD'],
  [LibraryItemType.AudioBook, 'Ljudbok'],
  [LibraryItemType.ReferenceBook, 'Referensbok'],
]);

type CreateLibraryItemInputModel = {
  categoryId: number;
  title: string;
  author?: string;
  pages?: number;
  runTimeMinutes?: number;
  type: LibraryItemType;
};

type UpdateLibraryItemInputModel = {
  categoryId?: number;
  title?: string;
  author?: string;
  pages?: number;
  runTimeMinutes?: number;
  borrower?: string;
  borrowDate?: string;
  type: LibraryItemType;
};

export {
  LibraryItemModel,
  LibraryItemType,
  libraryItemTypeName,
  CreateLibraryItemInputModel,
  UpdateLibraryItemInputModel,
};

