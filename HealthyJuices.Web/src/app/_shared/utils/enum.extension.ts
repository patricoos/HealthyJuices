import { SelectItem } from 'primeng/api';

export class EnumExtension {
  static getNamesAndValues<T extends number>(e: any): SelectItem[] {
    return EnumExtension.getNames(e).map(n => ({ name: n, value: e[n] as T }));
  }

  static getNames(e: any): string[] {
    return EnumExtension.getObjValues(e).filter(v => typeof v === 'string') as string[];
  }

  static getValues<T extends number>(e: any): T[] {
    return EnumExtension.getObjValues(e).filter(v => typeof v === 'number') as T[];
  }

  private static getObjValues(e: any): (number | string)[] {
    return Object.keys(e).map(k => e[k]);
  }
}
