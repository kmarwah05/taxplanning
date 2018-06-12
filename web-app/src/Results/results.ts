import { inject } from 'aurelia-framework';
import { HttpService } from 'HttpService';

@inject(HttpService)
export class Results {
  total: number = 4325;
  net: number = 2834

  constructor(private httpService: HttpService) {
    httpService.ConfigureClient();
  }

  CreateTable() {
  }

  GetResults() {
    let form = new FormData;
    var data = JSON.parse(sessionStorage.userData)
    //console.log(data)

    form.set('FilingStatus', data.FilingStatus);
    form.set('Income', data.Income);
    form.set('BasicAdjustment', '1000');
    form.set('RetirementDate', '423');
    form.set('EndOfPlanDate', '15332');
    form.set('CapitalGains', '423');
    form.set('FormAssets', this.BuildAssetString(data.Assets));

    this.httpService.SendPost(form);
  }

  BuildAssetString(array) {
    var assetString: string = "["

    for (let i = 0; i < array.length; i++) {
      assetString += ("[\"" + array[i].name + "\",\"" + array[i].type + "\",\"" + array[i].value + "\"]");

      if ((i + 1) <= array.length) { //while you have more assets add a comma
        assetString += ","
      }
    }

    assetString += "]"
    return assetString
  }
}
