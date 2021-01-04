import { Order } from 'src/app/management/models/order.model';
import { Product } from '../products/product.model';

export interface OrderProduct {
  id: number;
  orderId: number;
  productId: number;
  order: Order;
  product: Product;
  amount: number;
}
