export interface Product {
  id: number;
  name: string;
  description: string;

  unit: number;
  quantityPerUnit: number;
  defaultPricePerUnit: number;

  dateCreated: Date;
  dateModified: Date;
  isRemoved?: boolean;
}
