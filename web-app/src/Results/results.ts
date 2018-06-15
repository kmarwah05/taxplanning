import { inject } from 'aurelia-framework';
import { HttpService } from 'HttpService';
import { Chart } from 'chart.js';

@inject(HttpService)
export class Results {
  total: number = 4325;
  net: number = 2834;
  data = [];
  overall;

  constructor(private httpService: HttpService) {
    httpService.ConfigureClient(); //set the http client up only once when we start results
    this.GetResults();
  }

  AVFBuildTable() {
    var counter = 0;
    var carouselText = '<ol class="carousel-indicators">'
    var carouselInternal = ''
    var tableString = ''

    for (let i = 0; i < 3; i++) { //for loop for each page
      //only the first item needs to be active
      if (i == 0) {
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class="active"></li>' //Build carousel indicators based on how many assets we have
        carouselInternal += '<div class="item container active">'                                                    //Build carousel items based on how many assets we have
      }
      else {
        carouselInternal += '<div class="item container">'
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class=""></li>'
      }
      tableString = ''
      carouselInternal += '<div class ="charts row">'
      for (let i = 0; i < 2; i++) { //for loop for each table, on each page we have two tables
        carouselInternal += '<div class="col"><canvas id="chart' + counter + '">' +
          '</canvas></div>'
        if (i == 0) { tableString += '<div class="tables row">' }
        tableString +=
          '<div class="col">' +
          '<table class="table table-dark table-sm">' +
          '<caption id="tableCaption">' + this.data[counter].Solution + '</caption>' + //Caption the table with the solution
          '<thead>' +
          '<tr>' +
          '<th>Year</th>' +
          '<th>Amount</th>' +
          '<th>Additions</th>' +
          '<th>Withdrawls</th>' +
          '</tr>' +
          '</thead>' +
          '<tbody>';
        this.data[counter].YearlyDetails.forEach(element => { //add all the yearly data to the table
          tableString += '<tr><td>' + element.Year + '</td>' +
            '<td>' + element.YearlyAmount + '</td>' +
            '<td>' + element.YearlyTax + '</td></tr>'
        });
        tableString += '<tr>' +
          '</tr>' +
          '</tbody>' +
          '</table>' +
          '</div>'
        if (counter != this.data.length - 1) {
          counter++;
        }
      }
      carouselInternal += '</div>'
      carouselInternal += tableString
      carouselInternal += '</div><div class="carousel-caption d-none d-md-block">' + //add the name of the asset type to the page
        '<h5>' + this.data[counter - 1].Name + '</h5>' +
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

  BuildChart(counter) {
    for (let i = 0; i <= counter; i++) {
      var chart = document.getElementById('chart' + i);
      var years = []
      var amount = []
      this.data[i].YearlyDetails.forEach(element => {
        years = [...years, element.Year]
        amount = [...amount, element.YearlyAmount]
      });
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
      })
      .then(nothing => {
        console.log(this.data)
        this.BuildOverall()
      }) //build the table once the data is in
  }

  MakePage() {
    var counter = 0; //how many charts we need, one per table
    var carouselText = '<ol class="carousel-indicators">'
    var carouselInternal = ''
    var tableString = ''
    var overallTable = ''

    for (let i = 0; i < 3; i++) { //for loop for each page
      if (i == 0) { //build the overall page
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class="active"></li>' //Build carousel indicators based on how many assets we have
        carouselInternal += '<div class="item container active">'
        carouselInternal += '<div class="col"><canvas id="chart' + counter + '">' +
          '</canvas></div>'
        tableString += this.BuildTable(this.overall)
        tableString += this.FillTableRows(this.overall)
       
      }
      else {
        carouselInternal += '<div class="item container">'
        carouselText += '<li data-target="results#scheduleCarousel" data-slide-to="' + i + '" class=""></li>'

        tableString = '' //reset the table string for each page
        carouselInternal += '<div class ="charts row">' //create a row for charts so they are side by side
        for (let i = 0; i < 2; i++) { //on each page we have two tables
          carouselInternal += '<div class="col"><canvas id="chart' + counter + '">' +
            '</canvas></div>'
          if (i == 0) { tableString += '<div class="tables row">' } //create one table row so tables are side by side
          tableString += this.BuildTable(this.data[i])
          tableString += this.FillTableRows(this.data[i])
        }
      }
      tableString += '<tr>' +
      '</tr>' +
      '</tbody>' +
      '</table>' +
      '</div>'
      counter++;
      carouselInternal += '</div>'
      carouselInternal += tableString
      carouselInternal += '</div><div class="carousel-caption d-none d-md-block">' + //add the name of the asset type to the page
        '<h5>' + this.data[counter - 1].name + '</h5>' +
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
      "type": "Overall",
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
      if (element.prefered == true) { //find the best account options
        counter = 0;
        tAdditions += element.additions
        tWithdrawls += element.withdrawls
        tAfterTax += element.afterTaxWithdrawls
        tNetCash += element.netCashOut
        tTotalCash += element.totalCashOut

        element.YearlyAmount.forEach(element => {
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
    this.overall = overall
    this.MakePage()
  }

  FillTableRows(currentSet): string {
    var tableString: string = '';
    for (let j = 0; j < currentSet.years.length; j++) { //add all the yearly data to the table
      tableString += '<tr><td>' + currentSet.years[j] + '</td>' +
        '<td>' + currentSet.yearlyAmount[j] + '</td>'
    }
    return tableString
  }

  BuildTable(currentSet){
    var tableString: string = ''
    tableString +=
          '<div class="col">' +
          '<table class="table table-dark table-sm">' +
          '<caption id="tableCaption">' + currentSet.type + '</caption>' + //Caption the table with the type
          '<thead>' +
          '<tr>' +
          '<th>Year</th>' +
          '<th>Amount</th>' +
          '</tr>' +
          '</thead>' +
          '<tbody>';
    return tableString
  }
}
