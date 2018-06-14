import { inject } from 'aurelia-framework';
import { HttpService } from 'HttpService';

@inject(HttpService)
export class Results {
  total: number = 4325;
  net: number = 2834;
  data = [];

  constructor(private httpService: HttpService) {
    httpService.ConfigureClient(); //set the http client up only once when we start results
    this.GetResults();
  }

  BuildTable() {
    var counter = 0;
    var carouselText = '<ol class="carousel-indicators">'
    var carouselInternal = ''

    for (let i = 0; i < (this.data.length / 2); i++) { //for loop for each page
      //only the first item needs to be active
      if (i == 0) {
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class="active"></li>' //Build carousel indicators based on how many assets we have
        carouselInternal += '<div class="item container active">'                                                   //Build carousel items based on how many assets we have
      }
      else {
        carouselInternal += '<div class="item container">'
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class=""></li>'
      }
      //fake charts TODO: replace with actual chart
      carouselInternal += '<div id="chart" class="row">' +
        '<img class="col" src="https://previews.123rf.com/images/gibsonff/gibsonff1204/gibsonff120400099/13362368-growth-chart.jpg"/>' +
        '<img class="col" src="https://previews.123rf.com/images/gibsonff/gibsonff1204/gibsonff120400099/13362368-growth-chart.jpg"/>' +
        '</div>'
        +'<div class="tables row">' 
      for (let i = 0; i < 2; i++) { //for loop for each table, on each page we have two tables
        carouselInternal += 
          '<div class="col">' +
          '<table class="table table-dark table-sm">' +
          '<caption id="tableCaption">' + this.data[counter].Solution + '</caption>' + //Caption the table with the solution
          '<thead>' +
          '<tr>' +
          '<th>Year</th>' +
          '<th>Amount</th>' +
          '<th>Amount Change</th>' +
          '</tr>' +
          '</thead>' +
          '<tbody>';
        this.data[counter].YearlyDetails.forEach(element => { //add all the yearly data to the table
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
      carouselInternal += '</div><div class="carousel-caption d-none d-md-block">' + //add the name of the asset type to the page
        '<h5>' + this.data[counter-1].Name + '</h5>' +
        '</div>' +
        '</div>'
    }
    carouselInternal += '</div>'
    carouselText += '</ol>';
    //inject all the html
    document.getElementById("carouselAmount").innerHTML = carouselText
    document.getElementById("carouselInner").innerHTML = carouselInternal
  }

  GetResults() {
    let data = sessionStorage.userData
    console.log(data)

    this.SendPost(data)
  }

  BuildAssetString(array) {//back end wants assets to be formatted in an array of string arrays
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
        console.log(data)
        let obj;
        let element;
        let keyNames = Object.keys(data)
        //for each asset type sent from the api
        for (let i = 0; i < keyNames.length; i++) {
          var keyName = keyNames[i].split(" ") 
          obj = { //create an object to hold all the info coming in
            "Name": "",
            "Solution": "",
            "YearlyDetails": []
          }

          obj.Name = keyName[0] //overall,401k,ira,etc
          obj.Solution = keyName[1] //desired or optimal
          let table = data[Object.keys(data)[i]];
          for (let i = 0; i < table.years.length; i++) { //for each year get the specific details
            element = { //create an object to hold each years details
              "Year": table.years[i],
              "YearlyAmount": table.yearlyAmount[i],
              "YearlyTax": table.yearlyChange[i]        //TODO: Change to yearly change
            }
            obj.YearlyDetails = [...obj.YearlyDetails, element] //all the years in an array
          }
          this.data = [...this.data, obj] //add the completed obj to the data array
        }
        //console.log(this.data)
      })
      .then(nothing => this.BuildTable()) //build the table once the data is in
  }
}
