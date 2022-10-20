import { Component, OnDestroy, OnInit } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ClrLoadingState } from '@clr/angular';
import { Actions, ofActionSuccessful, Select, Store } from '@ngxs/store';
import { first, Observable } from 'rxjs';

import {
  ConfigurationMode,
  configurationModeName,
} from '@shared/constants/configuration-mode.enum';
import { FormService } from '@shared/services/form.service';

import {
  CategoryModel,
  CreateCategoryInputModel,
  UpdateCategoryInputModel,
} from '@models/category/category.model';
import { CategoryActions } from '@services/category/category.action';
import { CategoryState } from '@services/category/category.state';
import { LibraryItemActions } from '@services/library-item/library-item.action';

@Component({
  selector: 'category-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class CategoryConfigurationComponent implements OnInit, OnDestroy {
  // Access Enum in HTML
  ConfigurationModeEnum = ConfigurationMode;

  submitBtnState: ClrLoadingState = ClrLoadingState.DEFAULT;
  loadingState: ClrLoadingState = ClrLoadingState.DEFAULT;

  // Shows which mode the user is in
  configurationMode: ConfigurationMode = ConfigurationMode.Create;

  @Select(CategoryState.getCategory)
  category$: Observable<CategoryModel>;
  category = new CategoryModel();

  form = this.fb.group({
    categoryName: ['', Validators.required],
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
      this.getCategoryById(parseInt(id));

    this.updateForm();
  }

  ngOnDestroy(): void {
    this.store.dispatch(CategoryActions.ClearStore);
    this.store.dispatch(LibraryItemActions.ClearStore);
  }

  setConfigurationMode(id: string): void {
    if (!id) this.navigateBack();

    if (id === ConfigurationMode.Create) {
      this.configurationMode = ConfigurationMode.Create;
      return;
    }

    this.configurationMode = ConfigurationMode.Edit;
    this.getLibraryItems();
  }

  getCategoryById(id: number): void {
    this.store.dispatch(new CategoryActions.GetById(id));
  }

  getLibraryItems(): void {
    this.store.dispatch(new LibraryItemActions.Get());
  }

  // #region Form
  // Updates form as soon as GetById completes.
  updateForm(): void {
    switch (this.configurationMode) {
      case ConfigurationMode.Edit:
        this.actions$
          .pipe(ofActionSuccessful(CategoryActions.GetById))
          .subscribe({
            next: () => {
              this.category$.pipe(first()).subscribe({
                next: (category) => {
                  this.category = category;
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
        this.category = new CategoryModel();
        break;
    }

    this.loadingState = ClrLoadingState.SUCCESS;
  }

  updateFormValues(): void {
    this.form = FormService.fillFormWithData(this.form, this.category);
  }

  submit(): void {
    this.navigateBackOnSuccess();

    this.submitBtnState = ClrLoadingState.LOADING;

    const id = this.category.id;

    if (!id) {
      const inputModel: CreateCategoryInputModel = {
        categoryName: this.form.controls.categoryName.value,
      };

      this.createCategory(inputModel);
      return;
    }

    const inputModel: UpdateCategoryInputModel = {
      categoryName: this.form.controls.categoryName.value,
    };

    this.updateCategory(id, inputModel);
  }

  createCategory(inputModel: CreateCategoryInputModel): void {
    this.store.dispatch(new CategoryActions.Create(inputModel));

    this.submitBtnState = ClrLoadingState.DEFAULT;
  }

  updateCategory(id: number, inputModel: UpdateCategoryInputModel): void {
    this.store.dispatch(new CategoryActions.Update(id, inputModel));

    this.submitBtnState = ClrLoadingState.DEFAULT;
  }

  cancel(): void {
    this.navigateBack();
  }

  formIsInvalid(): boolean {
    return this.form.invalid || this.form.pristine;
  }
  // #endregion

  // #region Others
  isLoading(): boolean {
    return this.loadingState === ClrLoadingState.LOADING;
  }
  submitIsLoading(): boolean {
    return this.submitBtnState === ClrLoadingState.LOADING;
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
          CategoryActions.CreateSuccessful,
          CategoryActions.UpdateSuccessful
        )
      )
      .subscribe(() => {
        this.navigateBack();
      });
  }
  // #endregion
}
