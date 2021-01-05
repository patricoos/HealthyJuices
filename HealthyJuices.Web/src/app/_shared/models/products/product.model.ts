import { ProductUnitType } from '../enums/product-unit-type.enum';

export interface Product {
  id: number;
  name: string;
  description: string;

  unit: ProductUnitType;
  quantityPerUnit: number;
  defaultPricePerUnit: number;

  dateCreated: Date;
  dateModified: Date;
  isRemoved?: boolean;
  isActive: boolean;
}
