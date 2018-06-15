import { inject } from 'aurelia-framework';
import { HttpService } from 'HttpService';
import { Chart } from 'chart.js';

@inject(HttpService)
export class Results {
  total: number = 4325;
  net: number = 2834;
  data = [];

  constructor(private httpService: HttpService) {
    httpService.ConfigureClient(); //set the http client up only once when we start results
    this.GetResults();
  }

  BuildChart(counter) {
    console.log(counter)
    for (let i = 0; i <= counter; i++) {
      var chart = document.getElementById('chart' + i);
      var years = this.data[i].years
      var amount = this.data[i].yearlyAmount

      var myChart = new Chart(chart, {
        type: 'line',
        data: {
          labels: years,
          datasets: [{
            label: 'Total Value',
            data: amount,
            borderColor: 'rgba(255,99,132,1)',
            fill: false
          }]
        },
        options: {
          scales: {
            yAxes: [{
              ticks: {
                beginAtZero: true
              }
            }]
          }
        }
      });
    }
  }

  GetResults() {
    let data = sessionStorage.userData
    console.log(data)
    this.SendPost(data)
  }

  //sends the request to the api then formats the data for the table
  SendPost(form) {
    this.httpService.Fetch(form)
      .then(results => results.json())
      .then(data => {
        this.data = data
        console.log(this.data)
      })
      .then(nothing => {    
        this.BuildOverall()
      }) //build the table once the data is in
  }

  MakePage() {
    var counter = 0; //how many charts we need, one per table
    var carouselText = '<ol class="carousel-indicators">'
    var carouselInternal = ''
    var tableString = ''

    for (let i = 0; i < 3; i++) { //for loop for each page
      tableString = '' //reset the table string for each page
      if (i == 0) { //build the overall page
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class="active"></li>' //Build carousel indicators based on how many assets we have
        carouselInternal += '<div class="item container active">'
        carouselInternal += '<div class="col"><canvas id="chart' + counter + '">' +
          '</canvas></div>'
        tableString += this.BuildTable(this.data[counter]) //make the table inside the container
        counter++;
      }
      else {
        carouselInternal += '<div class="item container">'
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class=""></li>'
        carouselInternal += '<div class ="charts row">' //create a row for charts so they are side by side
        for (let i = 0; i < 2; i++) { //on each page we have two tables
          carouselInternal += '<div class="col"><canvas id="chart' + counter + '">' +
            '</canvas></div>'
          if (i == 0) { tableString += '</div><div class="tables row">' } //create one table row so tables are side by side
          tableString += this.BuildTable(this.data[counter])
          counter++;
        }
      }


      carouselInternal += tableString
      carouselInternal += '</div>'
      carouselInternal += '</div><div class="carousel-caption d-none d-md-block">' + //add the name of the asset type to the page
        '<h5>' + this.data[counter].name + '</h5>' +
        '</div>' +
        '</div>'
    }
    carouselInternal += '</div>'
    carouselText += '</ol>';
    //inject all the html
    document.getElementById("carouselAmount").innerHTML = carouselText
    document.getElementById("carouselInner").innerHTML = carouselInternal
    this.BuildChart(counter)
  }

  BuildOverall() {
    var tYears = this.data[0].years
    var amountOfYears = tYears.length
    var counter: number = 0;
    var tAdditions: number = 0;
    var tWithdrawls: number = 0;
    var tAfterTax: number = 0;
    var tNetCash: number = 0;
    var tTotalCash: number = 0;

    var overall = {
      "additions": 0,
      "name": "Overall",
      "assetType": "Overall",
      "afterTaxWithdrawls": 0,
      "withdrawls": 0,
      "netCashOut": 0,
      "totalCashOut": 0,
      "yearlyAmount": [],
      "years": []
    }

    //fastest way to zero out an array in javascript that I found on the internet
    for (var i = 0, tYearlyAmount = new Array(amountOfYears); i < amountOfYears;) tYearlyAmount[i++] = 0;
    this.data.forEach(element => {
      if (element.preferred == true) { //find the best account options
        counter = 0;
        tAdditions += element.additions
        tWithdrawls += element.withdrawls
        tAfterTax += element.afterTaxWithdrawls
        tNetCash += element.netCashOut
        tTotalCash += element.totalCashOut

        element.yearlyAmount.forEach(element => {
          tYearlyAmount[counter] += element
          counter++;
        });
      }
    })
    overall.additions = tAdditions
    overall.afterTaxWithdrawls = tAfterTax
    overall.netCashOut = tNetCash
    overall.totalCashOut = tTotalCash
    overall.withdrawls = tWithdrawls
    overall.yearlyAmount = tYearlyAmount
    overall.years = tYears
    this.data.unshift(overall)
    this.MakePage()
  }

  FillTableRows(currentSet): string {
    var tableString: string = '<tbody>';
    for (let j = 0; j < currentSet.years.length; j++) { //add all the yearly data to the table
      tableString += '<tr><td>' + currentSet.years[j] + '</td>' +
        '<td>' + currentSet.yearlyAmount[j] + '</td>'
      tableString += '</tr>'
    }
    tableString += '</tbody>'
    return tableString
  }

  BuildTable(currentSet) {
    var tableString: string = ''
    tableString +=
      '<div class="col">' +
      '<table class="table table-dark table-sm">' +
      '<caption id="tableCaption">' + currentSet.assetType + '</caption>' + //Caption the table with the type
      '<thead>' +
      '<tr>' +
      '<th>Year</th>' +
      '<th>Amount</th>' +
      '</tr>' +
      '</thead>'
    tableString += this.FillTableRows(currentSet)
    tableString += '</table></div>'
    return tableString
  }
}
