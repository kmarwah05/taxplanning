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
            mode:'cors',
            headers: {
              'content-type': 'multipart/form-data',
              'Accept': 'application/json'
            }
          })
    });
    this.client = client;
  }

  GetResults(){
    this.ConfigureClient();

    let form = new FormData;
    form.set('','');
    form.set('','');
    form.set('','');
    form.set('','');
    form.set('','');
    form.set('','');
    form.set('','');
    form.set('','');

    this.SendPost(form);
  }

  SendPost(form){
    this.client.fetch(this.endpoint,{
      method:"POST",
      body: form
    })
    .then(response => response.json())
    .then(data => {console.log(data)});
  }

}
