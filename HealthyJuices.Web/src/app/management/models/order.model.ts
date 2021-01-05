import { OrderProduct } from 'src/app/_shared/models/orders/order-product.model';
import { Company } from 'src/app/_shared/models/user/company.model';
import { User } from 'src/app/_shared/models/user/user.model';

export interface Order {
  id: number;
  dateCreated?: Date;
  dateModified?: Date;
  isRemoved?: boolean;

  deliveryDate: Date;

  userId: number;
  user: User;

  destinationCompanyId: number;
  destinationCompany: Company;

  orderProducts: OrderProduct[];

  userName?: string;
  productsNames?: string;
  destinationCompanyName?: string;
}
