import { inject } from 'aurelia-framework';
import { HttpService } from 'HttpService';

@inject(HttpService)
export class Results {
  total: number = 4325;
  net: number = 2834;
  data = [];

  constructor(private httpService: HttpService) {
    httpService.ConfigureClient();
    this.GetResults();
  }

  BuildTable() {
    //Build carousel indicators based on how many assets we have
    var counter = 0;
    var carouselText = '<ol class="carousel-indicators">'
    var carouselInternal = ''

    for (let i = 0; i < (this.data.length / 2); i++) {
      if (i == 0) {
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class="active"></li>'
        carouselInternal += '<div class="item container active">'
      }
      else {
        carouselInternal += '<div class="item container">'
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class=""></li>'
      }
      carouselInternal += '<div id="chart" class="row">' +
        '<img class="col" src="https://previews.123rf.com/images/gibsonff/gibsonff1204/gibsonff120400099/13362368-growth-chart.jpg"/>' +
        '<img class="col" src="https://previews.123rf.com/images/gibsonff/gibsonff1204/gibsonff120400099/13362368-growth-chart.jpg"/>' +
        '</div>'
        +'<div class="tables row">' 
      for (let i = 0; i < 2; i++) {
        //TODO: replace with actual chart

        carouselInternal += 
          '<div class="col">' +
          '<table class="table table-dark table-sm">' +
          '<caption id="tableCaption">' + this.data[counter].Solution + '</caption>' +
          '<thead>' +
          '<tr>' +
          '<th>Year</th>' +
          '<th>Amount</th>' +
          '<th>Amount Change</th>' +
          '</tr>' +
          '</thead>' +
          '<tbody>';
        this.data[counter].YearlyDetails.forEach(element => {
          carouselInternal += '<tr><td>' + element.Year + '</td>' +
            '<td>' + element.YearlyAmount + '</td>' +
            '<td>' + element.YearlyTax + '</td></tr>'
        });
        carouselInternal += '<tr>' +
          '</tr>' +
          '</tbody>' +
          '</table>' +
          '</div>' 
        if (counter != this.data.length - 1) {
          counter++;
        }
      }
      carouselInternal += '</div><div class="carousel-caption d-none d-md-block">' +
        '<h5>' + this.data[counter-1].Name + '</h5>' +
        '</div>' +
        '</div>'
    }
    carouselInternal += '</div>'
    carouselText += '</ol>';
    document.getElementById("carouselAmount").innerHTML = carouselText
    document.getElementById("carouselInner").innerHTML = carouselInternal
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
        let obj;
        let element;
        let keyNames = Object.keys(data)
        for (let i = 0; i < keyNames.length; i++) {
          var keyName = keyNames[i].split(" ")
          obj = {
            "Name": "",
            "Solution": "",
            "YearlyDetails": []
          }

          obj.Name = keyName[0]
          obj.Solution = keyName[1]

          let table = data[Object.keys(data)[i]];
          for (let i = 0; i < table.years.length; i++) {
            element = {
              "Year": table.years[i],
              "YearlyAmount": table.yearlyAmounts[i],
              "YearlyTax": table.yearlyTax[i]        //TODO: Change to yearly change
            }
            obj.YearlyDetails = [...obj.YearlyDetails, element]
          }
          this.data = [...this.data, obj]
        }
        console.log(this.data)
      })
      .then(nothing => this.BuildTable())
  }




  htmlString: string = '<div class="tables row">' +
    '<div class="col">' +
    '<table class="table table-dark table-sm" aurelia-table="data.bind: data[0].YearlyDetails; display-data.bind: $displayData" id="overallTable">' +
    '<thead>' +
    '<tr>' +
    '<th>Year</th>' +
    '<th>Amount</th>' +
    '<th>Amount Change</th>' +
    '</tr>' +
    '</thead>' +
    '<tbody>' +
    '<tr repeat.for="element of $displayData" compositionupdate.delegate="">' +
    '<td>${element.Year}</td>' +
    '<td>${element.YearlyAmount}</td>' +
    '<td>${element.YearlyTax}</td>' +
    '</tr>' +
    '<tr>' +
    '</tr>' +
    '</tbody>' +
    '</table>' +
    '</div>' +
    '<div class="col">' +
    '<table class="table table-dark table-sm" aurelia-table="data.bind: assets; display-data.bind: $displayData" id="overallTable">' +
    '<thead>' +
    '<tr>' +
    '<th>Year</th>' +
    '<th>Amount</th>' +
    '<th>Amount Change</th>' +
    '</tr>' +
    '</thead>' +
    '<tbody>' +
    '</tbody>' +
    '</table>' +
    '</div>' +
    '</div>' +
    '<div class="carousel-caption d-none d-md-block">' +
    '<h5>Overall</h5>' +
    '</div>' +
    '</div>'
}
