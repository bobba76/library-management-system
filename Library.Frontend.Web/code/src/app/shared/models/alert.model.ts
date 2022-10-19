export class Alert {
  id!: string;
  type!: AlertType;
  message!: string;
  closeable: boolean = true;
  autoClose?: boolean = false;
  keepAfterRouteChange?: boolean = false;
  fade: boolean = false;
  global: boolean = false;

  constructor(init?: Partial<Alert>) {
    Object.assign(this, init);
  }
}

export enum AlertType {
  Info = 'info',
  Success = 'success',
  Warning = 'warning',
  Error = 'danger',
}
