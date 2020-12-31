import { UserRole } from '../models/enums/user-role.enum';

export interface MenuItem {
  label: string;
  routerLink?: string;
  icon?: string;
  roles?: UserRole[];
  children?: MenuItem[];
}
