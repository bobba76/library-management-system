import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class FormService {
  public static fillFormWithData(form: FormGroup, data: any): FormGroup {
    form.patchValue(data);
    return form;
  }
}
