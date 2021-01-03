import { UnavailabilityReason } from "src/app/_shared/models/enums/unavailability-reason.enum";

export interface Unavailability {
  id: number;
  from: Date;
  to: Date;
  reason: UnavailabilityReason;
  comment: string;
}
