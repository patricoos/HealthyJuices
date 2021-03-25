import { UserRole } from '../enums/user-role.enum';
import { Company } from './company.model';

export interface User {
  id: string;

  created?: Date;
  lastModified?: Date;
  isRemoved: boolean;
  isActive: boolean;

  roles: UserRole;
  email: string;

  firstName: string;
  lastName: string;

  companyId?: number;
  company?: Company;
}
