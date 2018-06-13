import {HttpClient} from 'aurelia-fetch-client';

export class HttpService{
  endpoint: string = 'http://localhost:60419/api'
  client: HttpClient

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

  Fetch(form){
    return this.client.fetch(this.endpoint,{
      method:"POST",
      body: form
    })
  }
}
