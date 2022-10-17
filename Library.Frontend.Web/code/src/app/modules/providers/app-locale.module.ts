import { registerLocaleData } from '@angular/common';
import localeSwe from '@angular/common/locales/sv';
import { LOCALE_ID } from '@angular/core';

registerLocaleData(localeSwe);

export const locale = [{ provide: LOCALE_ID, useValue: 'sv' }];
