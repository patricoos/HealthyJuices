import { Params } from '@angular/router';

export interface BreadCrumb {
  label: string;
  url: string;
  params?: Params;
  canNavigateFromBreadcrumb?: boolean;
}
