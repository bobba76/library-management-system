import { locale as AppLocale } from '@modules/providers/app-locale.module';
import { services as AppServices } from '@modules/providers/app-service.module';

export const providers = [AppServices, AppLocale];
