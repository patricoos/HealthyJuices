export interface Company {
  id: number;

  name: string;
  comment: string;

  dateCreated?: Date;
  dateModified?: Date;
  isRemoved?: boolean;

  postalCode: string;
  city: string;
  street: string;

  latitude: number;
  longitude: number;
}
