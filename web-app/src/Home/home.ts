import { inject, NewInstance } from 'aurelia-framework';
import { ValidationRules, ValidationController } from 'aurelia-validation';

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
  message = '';
  errors = []
  incomeValidate: string = '';
  desiredAdditions: string = '';
  desiredWithdrawls: string = '';

  addButton() {
    //create a new asset and add it to the assets array
    let asset = { "name": this.name, "type": this.type, "value": this.value, "id": this.counter }
    this.assets = [...this.assets, asset]
    this.counter++;
    //reset the fields for reasons
    this.name = ''
    this.type = ''
    this.value = ''
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
    if(this.name.length != 0 && this.type.length !=0 && this.value.length !=0)
    {
      this.addButton()
    }
    sessionStorage.userData = JSON.stringify(
      {
        "FilingStatus": this.filingStatus,
        "Income": this.incomeValidate,
        "BasicAdjustment": this.basicAdjustment,
        "RetirementDate": this.retirementDate,
        "EndOfPlan": this.endOfPlan,
        "CapitalGains": this.capitalGains,
        "Assets": this.assets,
        "DesiredAdditions": this.desiredAdditions,
        "DesiredWithdrawls": this.desiredWithdrawls
      });
  }

  bind() {
    return this.assets
  }




  constructor(private controller: ValidationController) {
    ValidationRules
      .ensure((m: Home) => m.filingStatus).displayName("Filing Status").required()
      .ensure((m: Home) => m.incomeValidate).displayName("Income value").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.basicAdjustment).displayName("Basic Adjustment value").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.retirementDate).displayName("Retirement Date in yyyy").required().matches(new RegExp(/^[0-9]{4}$/))
      .ensure((m: Home) => m.capitalGains).displayName("Capital gains value").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.endOfPlan).displayName("End of Plan in yyyy").required().matches(new RegExp(/^[0-9]{4}$/))
      .ensure((m: Home) => m.desiredAdditions).displayName("Desired Additions").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.desiredWithdrawls).displayName("Desired Withdrawls").required().matches(new RegExp(/[0-9]/))
      .on(this);
  }



  validateButton() {
    this.jsonify()
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
