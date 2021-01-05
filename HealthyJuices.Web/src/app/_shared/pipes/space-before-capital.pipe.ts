import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'spaceBeforeCapital'
})
export class SpaceBeforeCapitalPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (value) {
      const result = value.replace(/([A-Z])/g, ' $1').trim();
      return result.charAt(0) + result.substring(1).toLowerCase();
    }
    return '';
  }

}
