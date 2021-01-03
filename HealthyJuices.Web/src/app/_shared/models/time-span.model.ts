export class TimeSpan {

  public ticks: number;


  public get days(): number {
    return Math.round(this.ticks / 864000000000);
  }

  public get hours(): number {
    return (this.ticks / 36000000000 % 24);
  }

  public get miliseconds(): number {
    return (this.ticks / 10000 % 1000);
  }

  public get minutes(): number {
    return (this.ticks / 600000000 % 60);
  }

  public get seconds(): number {
    return (this.ticks / 10000000 % 60);
  }

  public get totalDays(): number {
    return this.ticks * 1.15740740740741E-12;
  }

  public get totalHours(): number {
    return this.ticks * 2.77777777777778E-11;
  }

  public get totalMiliseconds(): number {
    const num = this.ticks * 0.0001;
    if (num > 922337203685477.0) {
      return 922337203685477.0;
    }
    if (num < -922337203685477.0) {
      return -922337203685477.0;
    }
    return num;
  }

  public get totalMinutes(): number {
    return +(this.ticks * 1.66666666666667E-09).toFixed(0);
  }

  public get totalSeconds(): number {
    return this.ticks * 1E-07;
  }

  get formatted() {
    return this.zeroPrependedHours + ':' + this.zeroPrependedMinutes + ':' + this.zeroPrependedSecond;
  }

  constructor(ticks: number) {
    this.ticks = ticks;
  }

  get zeroPrependedHours() {
    let result = '';
    if (this.hours < 10) {
      result += '0';
    }
    result += this.hours;

    return result;
  }

  public static localHour(t: TimeSpan) {
    const d = new Date();
    const gmtHours = -d.getTimezoneOffset() / 60;
    return (t.hours + gmtHours) === 24 ? 0 : t.hours + gmtHours;
  }
  static getFormattedWithSeconds(ts: TimeSpan): string {
    let result = '';
    if (ts.totalHours < 10) {
      result += '0';
    }
    result += Math.floor(ts.totalHours) + ':';
    if (ts.minutes < 10) {
      result += '0';
    }
    result += Math.floor(ts.minutes) + ':';
    if (ts.seconds < 10) {
      result += '0';
    }
    result += Math.floor(ts.seconds);
    return result;
  }
  static getFormatted(ts: TimeSpan): string {
    let result = '';
    if (ts.totalHours < 10) {
      result += '0';
    }
    result += Math.floor(ts.totalHours) + ':';
    if (ts.minutes < 10) {
      result += '0';
    }
    result += Math.floor(ts.minutes);
    return result;
  }

  static fromDate(date: Date): TimeSpan {
    const ticks = TimeSpan.timeToTicks(date.getHours(), date.getMinutes(), date.getSeconds());
    return new TimeSpan(ticks);
  }

  static fromMinutes(value: number): TimeSpan {
    return this.interval(value, 60000);
  }

  static fromSeconds(value: number): TimeSpan {
    const result = this.interval(value, 1000);
    return result;
  }

  static fromTicks(value: number) {
    return new TimeSpan(value);
  }

  static fromHours(value: number) {
    return this.interval(value, 3600000);
  }

  static interval(value: number, scale: number): TimeSpan {
    const num = value * scale + (value >= 0.0 ? 0.5 : -0.5);
    return new TimeSpan(num * 10000);
  }

  static toDate(time: TimeSpan): Date {
    const date = new Date(2000, 1, time.days, time.hours, time.minutes, time.seconds);
    return date;
  }

  static timeToTicks(hour: number, minute: number, second: number): number {
    const num = hour * 3600 + minute * 60 + second;
    return num * 10000000;
  }

  public get zeroPrependedMinutes() {
    let result = '';
    if (this.minutes < 10) {
      result += '0';
    }
    result += this.minutes;

    return result;
  }

  public get zeroPrependedSecond() {
    let result = '';
    if (this.seconds < 10) {
      result += '0';
    }
    result += this.seconds;

    return result;
  }

  public toHhMm(): string {
    let result = '';
    if (this.totalHours < 10) {
      result += '0';
    }
    result += Math.floor(this.totalHours) + ':';
    if (this.minutes < 10) {
      result += '0';
    }
    result += Math.floor(this.minutes) + ':';
    if (this.seconds < 10) {
      result += '0';
    }
    result += Math.floor(this.seconds);
    return result;
  }
}
