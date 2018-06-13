import {HttpClient} from 'aurelia-fetch-client';

export class HttpService{
  client: HttpClient
  endpoint: string = 'http://localhost:60419/api'
  
  ConfigureClient(){
    let client = new HttpClient;
    client.configure(config => {
      config
        .withBaseUrl(this.endpoint)
          .withDefaults({
            mode:'cors',
            headers: {
              'Access-Control-Allow-Headers':'*',
              'content-type': 'mulipart/form-data',
              'Accept': 'application/json'
            }
          })
    });
    this.client = client;
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
