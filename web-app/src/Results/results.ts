import { inject } from 'aurelia-framework';
import { HttpService } from 'HttpService';
import { Chart } from 'chart.js';
import noUiSlider from 'nouislider';
import wNumb from 'wnumb';

@inject(HttpService)
export class Results {
  total: number = 4325;
  net: number = 2834;
  data = [];
  max: number = 0;
  additionChange: string;

  constructor(private httpService: HttpService) {
    httpService.ConfigureClient(); //set the http client up only once when we start results
    this.GetResults();
  }

  BuildChart(counter) {
    for (let i = 0; i <= counter; i++) {
      var chart = document.getElementById('chart' + i);
      var years = this.data[i].years
      var amount = this.data[i].yearlyAmount
      var datasets = [];
      var dataset = {
        data: [],
        label: "",
        borderColor: "",
        backgroundColor: "",
        fill: false,
        pointBackgroundColor: ""
      }
      if (i == 0) {
        for (let j = 0; j < this.data.length; j++) {
          var randomColor = 'rgba(' + this.getRandomInt(256) + ',' + this.getRandomInt(256) + ',' + this.getRandomInt(256) + ',1)'
          dataset = {
            data: [],
            label: "",
            borderColor: "",
            backgroundColor: "",
            fill: false,
            pointBackgroundColor: ""
          }
          this.data[j].yearlyAmount.forEach(element => {
            dataset.data = [...dataset.data, element]
          });
          dataset.label = this.data[j].name
          dataset.pointBackgroundColor = randomColor
          dataset.borderColor = randomColor
          dataset.backgroundColor = randomColor
          dataset.fill = false
          datasets = [...datasets, dataset];
          console.log(this.data, dataset)
        }

      }
      else {
        datasets = [{
          label: 'Total Value',
          data: amount,
          borderColor: 'rgba(255,99,132,1)',
          backgroundColor: "rgba(255,99,132,1)",
          pointBackgroundColor: "rgba(255,99,132,1)",
          fill: false
        }
        ]
      }
      var myChart = new Chart(chart, {
        type: 'line',
        data: {
          labels: years,
          datasets: datasets
        },
        options: {
          scales: {
            yAxes: [{
              ticks: {
                max: (Math.ceil(this.max * 1.1 / 10000) * 10000),
                beginAtZero: true,

              }
            }]
          },
          hover: {
            mode: 'nearest',
            intersect: true
          },
          tooltips: {
            mode: 'index',
            intersect: false,
            callbacks: {
              label: function (tooltipItem, data) {
                var datasetLabel = data.datasets[tooltipItem.datasetIndex].label || 'Other';
                var label = (data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                return datasetLabel+': $'+label;
              }
            }
          }
        }
      });
    }
  }

  GetResults() {
    var data = sessionStorage.userData
    if (this.additionChange != null) {
      data = JSON.parse(data)
      data.desiredAdditions = this.additionChange
      data = JSON.stringify(data)
    }
    console.log(data)
    this.SendPost(data)
  }

  //sends the request to the api
  SendPost(form) {
    this.httpService.Fetch(form)
      .then(results => results.json())
      .then(data => {
        console.log(data)
        let temp = data
        data = data.map(x => x.yearlyAmount.map(e => e > 0 ? e : 0))
        for (let i = 0; i < data.length; i++) {
          temp[i].yearlyAmount = data[i]
        }
        this.data = temp
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
      if (i == 0) { //build the overall page, must be done as a special case since its a single table, also the first div needs to be marked active
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
        tableString += '</div><div class="tables row">' //create a table row so tables are side by side
        for (let i = 0; i < 2; i++) { //on each page we have two tables
          carouselInternal += '<div class="col"><canvas id="chart' + counter + '">' +
            '</canvas></div>'
          tableString += this.BuildTable(this.data[counter])
          counter++;
        }
      }
      carouselInternal += tableString
      carouselInternal += '<div class="carousel-caption d-none d-md-block">' + //add the name of the asset type to the page
        '<h5>' + this.data[counter - 1].name + '</h5>' +
        '</div></div></div>'
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

    var overall = { //combines the optimal accounts to see what the max payout is
      "additions": 0,
      "name": "Overall",
      "assetType": "Overall",
      "afterTaxWithdrawal": 0,
      "withdrawal": 0,
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
        tWithdrawls += element.withdrawal
        tAfterTax += element.afterTaxWithdrawal
        tNetCash += element.netCashOut
        tTotalCash += element.totalCashOut

        element.yearlyAmount.forEach(element => { //add up all the amounts for the perfered accounts
          tYearlyAmount[counter] += element
          counter++;
        });
      }
    })
    this.max = Math.max(...tYearlyAmount)
    overall.additions = tAdditions
    overall.afterTaxWithdrawal = tAfterTax
    overall.netCashOut = tNetCash
    overall.totalCashOut = tTotalCash
    overall.withdrawal = tWithdrawls
    overall.yearlyAmount = tYearlyAmount
    overall.years = tYears
    this.data.unshift(overall) //makes the overall table the first element
    this.MakePage() //once the overall table is done we can create the pages
  }

  FillTableRows(currentSet): string {
    var tableString: string = '<tbody>';
    for (let j = 0; j < currentSet.years.length; j++) { //add all the yearly data to the table
      tableString += '<tr><td>' + currentSet.years[j] + '</td>' +
        '<td>$' + this.numberWithCommas(currentSet.yearlyAmount[j].toFixed(2)) + '</td>'
      tableString += '</tr>'
    }
    tableString += '</tbody>'
    return tableString
  }

  BuildTable(currentSet) {
    var withdrawlsString = '<table class="table table-dark table-sm"><tr><th>Withdrawal rate</th><td>$' + this.numberWithCommas(currentSet.withdrawal.toFixed(2)) + '</td></tr><tr><th>After Tax withdrawal rate</th><td>$' + this.numberWithCommas(currentSet.afterTaxWithdrawal.toFixed(2)) + '</td></tr></table>'
    var totalString = '<table class="table table-dark table-sm"><tr><th>Total cash out</th><td>$' + this.numberWithCommas(currentSet.totalCashOut.toFixed(2)) + '</td></tr><tr><th>Net cash out</th><td>$' + this.numberWithCommas(currentSet.netCashOut.toFixed(2)) + '</td></tr></table>'
    var tableString: string = ''
    tableString +=
      '<div class="col">' +
      withdrawlsString +
      '<table class="table table-dark table-sm">' +
      '<caption id="tableCaption">' + currentSet.assetType + '</caption>' + //Caption the table with the type
      '<thead>' +
      '<tr align="center">' +
      '<th>Year</th>' +
      '<th>Amount</th>' +
      '</tr>' +
      '</thead>'
    tableString += this.FillTableRows(currentSet)
    tableString += '</table>' + totalString + '</div>'
    return tableString
  }

  attached() {
    var storage = JSON.parse(sessionStorage.userData)
    var min = 0;
    var max = parseInt(storage.income)
    var from = parseInt(storage.desiredAdditions)
    var range: noUiSlider = <noUiSlider>document.getElementById("range")
    var self = this;

    noUiSlider.create(range,{
      start: from,
      range: {
        min: min,
        max: max
      },
      tooltips: true,
      connect: true,
      step: 100,
      format: wNumb({
        decimals: 0
      }),
      pips: {
        mode: 'range',
        density: 10
      }
    });

    range.noUiSlider.on('set', function () {
      self.additionChange = range.noUiSlider.get()
      self.GetResults()
    });
  }

  UpdateAdditions() {
    this.additionChange = document.getElementById("add").innerText
    this.GetResults()
  }

  getRandomInt(max) {
    return Math.floor(Math.random() * Math.floor(max)); //thanks internet
  }
  numberWithCommas = (x) => {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","); //thanks internet
  }
}
