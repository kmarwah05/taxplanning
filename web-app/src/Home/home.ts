import { inject, NewInstance, children } from 'aurelia-framework';
import { ValidationRules, ValidationController } from 'aurelia-validation';
import $ from '../../node_modules/jquery/dist/jquery.js';
import 'aurelia-ion-rangeslider';




@inject(NewInstance.of(ValidationController))
export class Home {
  counter: number = 0;
  assets = [];
  name: string = '';
  type: string = '';
  value: string = '';
  filingStatus: string;
  income: string = '';
  retirementDate: string;
  endOfPlan: string;
  message = '';
  errors = []
  desiredAdditions: string = '';
  match: string = '';
  cap: string = '';
<<<<<<< HEAD
  children: string = '';
  currentAge: string = '';
  tChildren = [];
  age: number;
  childId: number = 0;
=======
  Tchildren = [];
  age: number = -1;
>>>>>>> 3621503d09b11f8cd27865edf2d7bc81befd4fdc

  addButton() {
    //create a new asset and add it to the assets array
    let asset = { "name": this.name, "type": this.type, "value": this.value, "match": this.match, "cap": this.cap, "id": this.counter }
    this.assets = [...this.assets, asset]
    this.counter++;
    //reset the fields
    this.name = ''
    this.type = ''
    this.value = ''
    this.match = ''
    this.cap = ''
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
    if (this.name.length != 0 && this.type.length != 0 && this.value.length != 0) {
      this.addButton()
    }
    var t = document.getElementById("test").innerText.split(",")
    this.retirementDate = t[0]
    this.endOfPlan = t[1]
    sessionStorage.userData = JSON.stringify(
      {
        "filingStatus": this.filingStatus,
        "income": this.income,
        "retirementDate": this.retirementDate,
        "endOfPlanDate": this.endOfPlan,
        "assets": this.assets,
        "desiredAdditions": this.desiredAdditions,
        "childrensAges": this.Tchildren
      });
  }

  bind() {
    return this.assets
  }

  constructor(private controller: ValidationController) {
    ValidationRules
      .ensure((m: Home) => m.filingStatus).displayName("Filing Status").required()
      .ensure((m: Home) => m.income).displayName("Income value").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.desiredAdditions).displayName("Additions").required().matches(new RegExp(/[0-9]/))
      .ensure((m: Home) => m.currentAge).displayName("Current Age").required().matches(new RegExp(/[0-9]/))
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

  addChildren() {
    if (this.age != -1) {
      this.Tchildren = [...this.Tchildren, this.age]
      console.log("Age ",this.age)
    }
    this.age = -1
  }

  // sliders starts here
  attached() {
    var from = new Date().getFullYear() + 1
    var to = from + 80

    $(function () {
      $('#range').ionRangeSlider({
        min: from,
        max: to,
        from: from,
        to: to,
        type: 'int',
        grid: true,
        grid_num: 10,
        prettify_enabled: false,
        onChange: function (data) {
          $('#test').prop("innerText", [data.from, data.to])
        },
        onStart: function (data) {
          $('#test').prop("innerText", [data.from, data.to])
        }
      });
    });
<<<<<<< HEAD
      // sliders ends here
    $('#Atype').change(function(){
      if($('#Atype option:selected').val() == "Roth 401k" || $('#Atype option:selected').val() == "401k"){
        $('#Matchtext').css("display","block");
        $('#Ematch').css("display","block");
        $('#Ecap').css("display","block");
        $('#Captext').css("display","block");
=======
    $('#Atype').change(function () {
      if ($('#Atype option:selected').val() == "Roth 401k" || $('#Atype option:selected').val() == "401k") {
        $('#Matchtext').css("display", "block");
        $('#Ematch').css("display", "block");
        $('#Ecap').css("display", "block");
        $('#Captext').css("display", "block");
>>>>>>> 3621503d09b11f8cd27865edf2d7bc81befd4fdc
      }
      else {
        $('#Matchtext').css("display", "none");
        $('#Ematch').css("display", "none");
        $('#Ecap').css("display", "none");
        $('#Captext').css("display", "none");
      }
    });
    $('#filing').change(function () {
      if ($('#filing option:selected').val() == "Joint") {
        $('#incomeRange').text("Enter Joint Income:")
      }
      else {
        $('#incomeRange').text("Enter Income:")
      }
    });
  }

  addChildren() {
    var child = {"age":this.age,"id":this.childId}
    this.tChildren = [...this.tChildren, child]
    this.childId++;
    }
     
    removeChild(id){
    let i = 0;
    this.tChildren.splice(id, 1); //remove the element that you clicked
    this.tChildren.forEach(element => { //fix the array index
    element.id = i
    i++;
    });
    this.childId--;
    }

}
