import { Company } from '../user/company.model';
import { UsersProductsReport } from './users-products-report.model';

export interface OrderReport {
  company: Company;
  productsByUser: UsersProductsReport[];
}
