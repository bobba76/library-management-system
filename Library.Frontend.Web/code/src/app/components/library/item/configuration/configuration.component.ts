import { Component, OnDestroy, OnInit } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ClrLoadingState } from '@clr/angular';
import { Actions, ofActionSuccessful, Select, Store } from '@ngxs/store';
import { first, Observable } from 'rxjs';

import {
  ConfigurationMode,
  configurationModeName
} from '@shared/constants/configuration-mode.enum';
import { FormService } from '@shared/services/form.service';

import {
  CreateLibraryItemInputModel,
  LibraryItemModel,
  LibraryItemType,
  UpdateLibraryItemInputModel
} from '@models/library-item/library-item.model';
import { LibraryItemActions } from '@services/library-item/library-item.action';
import { LibraryItemState } from '@services/library-item/library-item.state';

import { CategoryModel } from '@models/category/category.model';
import { CategoryActions } from '@services/category/category.action';
import { CategoryState } from '@services/category/category.state';

@Component({
  selector: 'library-item-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class LibraryItemConfigurationComponent implements OnInit, OnDestroy {
  // Access LibraryItemType in HTML
  LibraryItemTypeEnum = LibraryItemType;
  ConfigurationModeEnum = ConfigurationMode;

  submitBtnState: ClrLoadingState = ClrLoadingState.DEFAULT;
  loadingState: ClrLoadingState = ClrLoadingState.DEFAULT;

  // Shows which mode the user is in
  configurationMode: ConfigurationMode = ConfigurationMode.Create;

  @Select(LibraryItemState.getLibraryItem)
  libraryItem$: Observable<LibraryItemModel>;
  @Select(CategoryState.getCategories) categories$: Observable<
    Array<CategoryModel>
  >;
  libraryItem = new LibraryItemModel();

  form = this.fb.group({
    categoryId: [0, Validators.required],
    title: ['', Validators.required],
    author: [''],
    pages: [0],
    runTimeMinutes: [0],
    type: [LibraryItemType.Book, Validators.required],
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: NonNullableFormBuilder,
    private store: Store,
    private actions$: Actions
  ) {}

  ngOnInit() {
    this.loadingState = ClrLoadingState.LOADING;

    const id = this.route.snapshot.paramMap.get('id');

    this.setConfigurationMode(id);

    if (this.configurationMode === ConfigurationMode.Edit)
      this.getLibraryItemById(parseInt(id));

    this.updateForm();

    // We need all the employees to be able to set Manager.
    this.getCategories();
  }

  ngOnDestroy(): void {
    this.store.dispatch(LibraryItemActions.ClearStore);
  }

  setConfigurationMode(id: string): void {
    if (!id) this.navigateBack();

    if (id === ConfigurationMode.Create) {
      this.configurationMode = ConfigurationMode.Create;
      return;
    }

    this.configurationMode = ConfigurationMode.Edit;
  }

  getLibraryItemById(id: number): void {
    this.store.dispatch(new LibraryItemActions.GetById(id));
  }

  getCategories(): Observable<Array<CategoryModel>> {
    return this.store.dispatch(CategoryActions.Get);
  }

  // #region Form
  // Updates form as soon as GetById completes.
  updateForm(): void {
    switch (this.configurationMode) {
      case ConfigurationMode.Edit:
        this.actions$
          .pipe(ofActionSuccessful(LibraryItemActions.GetById))
          .subscribe({
            next: () => {
              this.libraryItem$.pipe(first()).subscribe({
                next: (libraryItem) => {
                  this.libraryItem = libraryItem;
                  this.updateFormValues();
                },
                error: (err: any) => {
                  console.error(err);
                  this.navigateBack();
                },
              });
            },
            error: (err) => {
              console.error(err);
              this.navigateBack();
            },
          });

        break;

      case ConfigurationMode.Create:
      default:
        this.libraryItem = new LibraryItemModel();
        this.form.controls.categoryId.setValue(null);
        break;
    }

    this.loadingState = ClrLoadingState.SUCCESS;
  }

  updateFormValues(): void {
    this.form = FormService.fillFormWithData(this.form, this.libraryItem);

    switch (this.libraryItem.type) {
      case LibraryItemType.Book:
        this.form.controls.runTimeMinutes.disable();
        break;
      case LibraryItemType.AudioBook:
        this.form.controls.author.disable();
        this.form.controls.pages.disable();
        break;
      case LibraryItemType.Dvd:
        this.form.controls.author.disable();
        this.form.controls.pages.disable();
        break;
      case LibraryItemType.ReferenceBook:
        this.form.controls.runTimeMinutes.disable();
        break;
    }
  }

  submit(): void {
    this.navigateBackOnSuccess();

    this.submitBtnState = ClrLoadingState.LOADING;

    switch (this.form.controls.type.value) {
      case LibraryItemType.Book:
        this.form.controls.runTimeMinutes.setValue(null);
        break;
      case LibraryItemType.AudioBook:
        this.form.controls.author.setValue(null);
        this.form.controls.pages.setValue(null);
        break;
      case LibraryItemType.Dvd:
        this.form.controls.author.setValue(null);
        this.form.controls.pages.setValue(null);
        break;
      case LibraryItemType.ReferenceBook:
        this.form.controls.runTimeMinutes.setValue(null);
        break;
    }

    const id = this.libraryItem.id;

    if (!id) {
      const inputModel: CreateLibraryItemInputModel = {
        categoryId: this.form.controls.categoryId.value,
        title: this.form.controls.title.value,
        author: this.form.controls.author.value,
        pages: this.form.controls.pages.value,
        runTimeMinutes: this.form.controls.runTimeMinutes.value,
        type: this.form.controls.type.value,
      };

      this.createLibraryItem(inputModel);
      return;
    }

    const inputModel: UpdateLibraryItemInputModel = {
      categoryId: this.form.controls.categoryId.value,
      title: this.form.controls.title.value,
      author: this.form.controls.author.value,
      pages: this.form.controls.pages.value,
      runTimeMinutes: this.form.controls.runTimeMinutes.value,
      type: this.form.controls.type.value,
    };

    this.updateLibraryItem(id, inputModel);
  }

  createLibraryItem(inputModel: CreateLibraryItemInputModel): void {
    this.store.dispatch(new LibraryItemActions.Create(inputModel));

    this.submitBtnState = ClrLoadingState.DEFAULT;
  }

  updateLibraryItem(id: number, inputModel: UpdateLibraryItemInputModel): void {
    this.store.dispatch(new LibraryItemActions.Update(id, inputModel));

    this.submitBtnState = ClrLoadingState.DEFAULT;
  }

  cancel(): void {
    this.navigateBack();
  }

  formIsInvalid(): boolean {
    return this.form.invalid || this.form.pristine || this.typeOfBookFormInvalid();
  }

  typeOfBookFormInvalid(): boolean {
    return (
      this.typeOfBookChosen() &&
      (!this.form.controls.pages.value || !this.form.controls.author.value)
    );
  }

  typeOfBookChosen(): boolean {
    return (
      this.form.controls.type.value === LibraryItemType.Book ||
      this.form.controls.type.value === LibraryItemType.ReferenceBook
    );
  }

  typeOfMediaChosen(): boolean {
    return (
      this.form.controls.type.value === LibraryItemType.AudioBook ||
      this.form.controls.type.value === LibraryItemType.Dvd
    );
  }
  // #endregion

  // #region Others
  isLoading(): boolean {
    return this.loadingState === ClrLoadingState.LOADING;
  }
  submitIsLoading(): boolean {
    return this.submitBtnState === ClrLoadingState.LOADING;
  }

  switchedType(type: LibraryItemType): void {
    switch (type) {
      case LibraryItemType.Book:
        setTimeout(() => {
          this.form.controls.runTimeMinutes.disable();
          this.form.controls.author.enable();
          this.form.controls.pages.enable();
        });
        break;

      case LibraryItemType.AudioBook:
        setTimeout(() => {
          this.form.controls.author.disable();
          this.form.controls.pages.disable();
          this.form.controls.runTimeMinutes.enable();
        });
        break;

      case LibraryItemType.Dvd:
        setTimeout(() => {
          this.form.controls.author.disable();
          this.form.controls.pages.disable();
          this.form.controls.runTimeMinutes.enable();
        });
        break;

      case LibraryItemType.ReferenceBook:
        setTimeout(() => {
          this.form.controls.runTimeMinutes.disable();
          this.form.controls.author.enable();
          this.form.controls.pages.enable();
        });
        break;
    }
  }

  getConfigurationModeName(mode: ConfigurationMode): string {
    return configurationModeName.get(mode) ?? '';
  }

  navigateBack(): void {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  isEditMode(): boolean {
    return this.configurationMode === this.ConfigurationModeEnum.Edit;
  }

  navigateBackOnSuccess(): void {
    this.actions$
      .pipe(
        ofActionSuccessful(
          LibraryItemActions.CreateSuccessful,
          LibraryItemActions.UpdateSuccessful
        )
      )
      .subscribe(() => {
        this.navigateBack();
      });
  }
  // #endregion
}
