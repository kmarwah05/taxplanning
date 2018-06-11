import {HttpClient} from 'aurelia-fetch-client';

export class Results{
  yearStart: number
  yearEnd: number
  client: HttpClient
  endpoint: string = 'http://localhost:60419/api'
  
  CreateTable() {
    var tableContent: string = "";
    for(var i = this.yearStart; i < this.yearEnd; i++){
      tableContent += "<tr>";
      tableContent += "<td>"+i+"</td>"

      tableContent += "</tr>";
    }
  }

  ConfigureClient(){
    let client = new HttpClient;
    client.configure(config => {
      config
        .withBaseUrl(this.endpoint)
          .withDefaults({
            mode:'no-cors',
            headers: {
              'content-type': 'mulipart/form-data',
              'Accept': 'application/json'
            }
          })
    });
    this.client = client;
  }

  GetResults(){
    this.ConfigureClient();

    let form = new FormData;
    form.set('FilingStatus','FilingStatus.Joint');
    form.set('Income','382');
    form.set('BasicAdjustment','1000');
    form.set('RetirementDate','423');
    form.set('EndOfPlanDate','15332');
    form.set('CapitalGains','423');
    form.set('FormAssets','[["Tim","RothIra","421423"]]');

    this.SendPost(form);
  }

  SendPost(form){
    console.log(form.get('FormAssets'))
    this.client.fetch(this.endpoint,{
      method:"POST",
      body: form
    })
    .then(response => response.json())
    .then(data => {console.log(data)});
  }

}
