import { OrderProduct } from './order-product.model';
import { OrderReport } from './order-report.model';

export interface DashboardOrderReport {
  orderReports: OrderReport[];
  products: OrderProduct[];
}
