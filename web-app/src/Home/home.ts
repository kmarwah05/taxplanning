export class Home {
  counter: number = 0;
  assets = [];
  name: string
  type: string
  value: string
  filingStatus: string
  income: string

  addButton() {
    //create a new asset and add it to the assets array
    let asset = { "name": this.name, "type": this.type, "value": this.value, "id":this.counter }
    this.assets = [...this.assets, asset]
    this.counter++;
    //console.log("adding ", this.assets);
    this.jsonify() //save data when a change is made
  }

  removeButton(id){
    let i = 0;
    //console.log("Removing",id);
    this.assets.splice(id,1);         //remove the element that you clicked
    this.assets.forEach(element => {  //fix the array index
      element.id = i
      i++;
    });
    this.counter--;
    this.jsonify() //save data when a change is made
  }

  //stores data so that it can be used on the results page
  jsonify(){
    sessionStorage.userData = JSON.stringify(
      {
      "FilingStatus":this.filingStatus,
      "Income":this.income,
      "Assets":this.assets});
  }

  bind() {
    return this.assets
  }


}
