import { Home } from './home';
import { inject, NewInstance } from 'aurelia-framework';
import { ValidationRules, ValidationController } from 'aurelia-validation';
import $ from '../../node_modules/jquery/dist/jquery.js';
import 'aurelia-ion-rangeslider';
;


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
  retirementDate: string;
  endOfPlan: string;
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
    this.jsonify()
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
    // if (this.name.length != 0 && this.type.length != 0 && this.value.length != 0) {
    //   this.addButton()
    // }
    var t = document.getElementById("test").innerText.split(",")
    this.retirementDate = t[0]
    this.endOfPlan = t[1]
    console.log("T: ",t," RD: ",this.retirementDate," EOP: ",this.endOfPlan)
    sessionStorage.userData = JSON.stringify(
      {
        "filingStatus": this.filingStatus,
        "income": this.incomeValidate,
        "basicAdjustment": this.basicAdjustment,
        "retirementDate": this.retirementDate,
        "endOfPlanDate": this.endOfPlan,
        "capitalGains": this.capitalGains,
        "assets": this.assets,
        "desiredAdditions": this.desiredAdditions,
        "desiredWithdrawals": this.desiredWithdrawls
      });
      console.log(sessionStorage.userData = JSON.stringify(
        {
          "filingStatus": this.filingStatus,
          "income": this.incomeValidate,
          "basicAdjustment": this.basicAdjustment,
          "retirementDate": this.retirementDate,
          "endOfPlanDate": this.endOfPlan,
          "capitalGains": this.capitalGains,
          "assets": this.assets,
          "desiredAdditions": this.desiredAdditions,
          "desiredWithdrawals": this.desiredWithdrawls
        }))
  }

  bind() {
    return this.assets
  }


  constructor(private controller: ValidationController) {
    ValidationRules
      .ensure((m: Home) => m.filingStatus).displayName("Filing Status").required()
      .ensure((m: Home) => m.incomeValidate).displayName("Income value").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.basicAdjustment).displayName("Basic Adjustment value").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.capitalGains).displayName("Capital gains value").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.desiredAdditions).displayName("Additions").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.retirementDate).displayName("Retirement Date and End of Plan").required()
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



  // sliders starts here

  attached() {
    var from = new Date().getFullYear()
    var to = this.endOfPlan

    var date = new Date().getFullYear()
    $(function () {
      $('#range').ionRangeSlider({
        min: date,
        max: date + 80,
        from: from,
        to: to,
        type: 'int',
        grid: true,
        grid_num: 10,
        onChange: function (data){
          $('#test').prop("innerText",[data.from, data.to])
        },
        onStart: function (data){
          $('#test').prop("innerText",[data.from, data.to])
        }
      });
    });

  }


  // sliders ends here

}

 /*
    $('#range').on("change", function () {
      var $this = $(this),
        value = $this.prop("value").split(";");
      this.retirementDate = value[0]
      console.log("ON FINISH:", this.retirementDate)
      this.endOfPlan = value[1] 
      this.save(value[0],value[1])
    })
    
     save(from , to){
    this.retirementDate = from;
    this.endOfPlan = to;
    */
