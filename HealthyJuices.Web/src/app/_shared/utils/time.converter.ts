import * as moment from 'moment';

export class TimeConverter {
  public static get tomorrow(): Date {
    return moment().add('days', 1).toDate();
  }

  public static toUtc(value: Date): Date | null {
    if (value == null) {
      return null;
    }

    return moment.utc(value).local().toDate();
  }

  public static toUtcString(value: Date): string {
    const date = this.toUtc(value);
    return moment(date).format('YYYY-MM-DD HH:mm');
  }

  public static isStringISOValid(date: string): boolean {
    return moment(date, moment.ISO_8601, true).isValid();
  }

  public static toLocalUnix(date: Date): number {
    const localTicks = moment.utc(date).local().unix() * 1000;
    return localTicks;
  }

  public static toLocal(date: Date): moment.Moment {
    const local = moment.utc(date).local();
    return local;
  }

  public static getCurrentWeekDateRange(): Date[] {
    return [this.getFirstDayOfThisWeek(), this.getLastDayOfThisWeek()];
  }

  public static getFromNowTillEndOfWeekDateRange(): Date[] {
    return [moment().toDate(), this.getLastDayOfThisWeek()];
  }

  public static getCurrentTwoWeeksDateRange(): Date[] {
    return [this.getFirstDayOfThisWeek(moment().subtract(7, 'days').toDate()), this.getLastDayOfThisWeek()];
  }

  public static getFirstDayOfThisWeek(date?: Date): Date {
    return this.getMinTime(moment(date).weekday(1).toDate());
  }

  public static getLastDayOfThisWeek(date?: Date): Date {
    return this.getMaxTime(moment(date).weekday(7).toDate());
  }

  public static getMinTime(date: Date): Date {
    return moment(date).hour(0).minute(0).second(0).millisecond(0).toDate();
  }

  public static getMaxTime(date: Date): Date {
    return moment(date).hour(23).minute(59).second(59).millisecond(999).toDate();
  }
}
