export interface Company {
  id: string;

  name: string;
  comment: string;

  created?: Date;
  lastModified?: Date;
  isRemoved?: boolean;

  postalCode: string;
  city: string;
  street: string;

  latitude: number;
  longitude: number;
}
