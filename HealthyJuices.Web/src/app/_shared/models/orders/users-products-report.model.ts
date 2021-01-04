import { User } from '../user/user.model';
import { OrderProduct } from './order-product.model';

export interface UsersProductsReport {
  user: User;
  productsByUser: UsersProductsReport;
  products: OrderProduct;
}
