export interface Company {
  id: string;

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
