import { inject } from 'aurelia-framework';
import { HttpService } from 'HttpService';

@inject(HttpService)
export class Results {
  total: number = 4325;
  net: number = 2834
  data = [];

  constructor(private httpService: HttpService) {
    httpService.ConfigureClient();
    this.GetResults();
  }

  GetResults() {
    let form = new FormData;
    let data = JSON.parse(sessionStorage.userData)
    //console.log(data)

    form.set('FilingStatus', data.FilingStatus);
    form.set('Income', data.Income);
    form.set('BasicAdjustment', data.BasicAdjustment);
    form.set('RetirementDate', data.RetirementDate);
    form.set('EndOfPlanDate', data.EndOfPlanDate);
    form.set('CapitalGains', data.CapitalGains);
    form.set('FormAssets', this.BuildAssetString(data.Assets));

    this.SendPost(form)
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

  //sends the request to the api then formats the data for the table
  SendPost(form) {
    this.httpService.Fetch(form)
      .then(results => results.json())
      .then(data => {
        let element = {
          "year": data.year,
          "totalCashOut": data.totalCashOut,
          "netCashOut": data.totalCashOut,
          "yearlyAmount": data.yearlyAmount,
          "yearlyChange": data.yearlyTax //will change to yearlyChange
        }
        console.log(JSON.stringify(data))
        this.data = [...this.data, element]
      })
  }
}
