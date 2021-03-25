import { ProductUnitType } from '../enums/product-unit-type.enum';

export interface Product {
  id: string;
  name: string;
  description: string;

  unit: ProductUnitType;
  quantityPerUnit: number;
  defaultPricePerUnit: number;

  created: Date;
  lastModified: Date;
  isRemoved?: boolean;
  isActive: boolean;
}
