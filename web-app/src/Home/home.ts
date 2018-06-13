import {inject, NewInstance} from 'aurelia-framework';
import {ValidationRules, ValidationController} from "aurelia-validation";

@inject(NewInstance.of(ValidationController))
export class Home {
  counter: number = 0;
  assets = [];
  name: string
  type: string
  value: string
  filingStatus: string
  income: string
  basicAdjustment: string = '';
  retirementDate: string
  endOfPlan: string
  capitalGains: string

  addButton() {
    //create a new asset and add it to the assets array
    let asset = { "name": this.name, "type": this.type, "value": this.value, "id": this.counter }
    this.assets = [...this.assets, asset]
    this.counter++;
    //console.log("adding ", this.assets);
  }

  removeButton(id) {
    let i = 0;
    //console.log("Removing",id);
    this.assets.splice(id, 1);         //remove the element that you clicked
    this.assets.forEach(element => {  //fix the array index
      element.id = i
      i++;
    });
    this.counter--;
  }

  //stores data so that it can be used on the results page
  jsonify() {
    sessionStorage.userData = JSON.stringify(
      {
        "FilingStatus": this.filingStatus,
        "Income": this.income,
        "BasicAdjustment": this.basicAdjustment,
        "RetirementDate": this.retirementDate,
        "EndOfPlan": this.endOfPlan,
        "CapitalGains": this.capitalGains,
        "Assets": this.assets
      });
    window.location.href = "http://localhost:8080/results"
  }

  bind() {
    return this.assets
  }

  message = '';
  errors = []
  incomeValidate: string = '';

  constructor(private controller: ValidationController) {
    ValidationRules
     // .ensure((m: Home) => m.fStatus).displayName("Income value in number").required()
      .ensure((m: Home) => m.incomeValidate).displayName("Income value in numeric value").required()
      .ensure((m: Home) => m.basicAdjustment).displayName("Basic Adjustment value in numeric value").required()
      .ensure((m: Home) => m.retirementDate).displayName("Retirement Date in yyyy").required()
      .ensure((m: Home) => m.capitalGains).displayName("Capital gains value in numeric value").required()
      .ensure((m: Home) => m.endOfPlan).displayName("End of Plan in yyyy").required()
      .on(this);
  }


  validateButton() {
    this.controller
      .validate()
      .then(v => {
        if (v.valid)
        window.location.href = "http://localhost:8080/results"
        else
          this.message = "You have errors!";
          this.errors = v.results;
      })
  }
  
}
