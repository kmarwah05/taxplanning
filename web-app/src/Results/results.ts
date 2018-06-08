import {HttpClient} from 'aurelia-fetch-client';

export class Results{
  yearStart: number
  yearEnd: number
  
  CreateTable() {
    const endpointL: string = "kraken:" //update with api endpoint
    var tableContent: string = "";
    for(var i = this.yearStart; i < this.yearEnd; i++){
      tableContent += "<tr>";
      tableContent += "<td>"+i+"</td>"

      tableContent += "</tr>";
    }
  }
}
