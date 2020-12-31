export interface Company {
  id: number;

  dateCreated?: Date;
  dateModified?: Date;
  isRemoved?: boolean;

  postalCode: string;
  city: string;
  street: string;

  latitude: number;
  longitude: number;
}
