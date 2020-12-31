import { environment } from 'src/environments/environment';

export abstract class BaseService {

  protected baseUrl: string;

  constructor() {
    this.baseUrl = environment.baseUrl;
  }

  protected isStatusSucceed(status: number): boolean {
    return status === 200;
  }
}
