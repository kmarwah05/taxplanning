import { inject, NewInstance } from 'aurelia-framework';
import { ValidationRules, ValidationController, RenderInstruction } from 'aurelia-validation';
import noUiSlider from 'nouislider';
import wNumb from 'wnumb';

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
  match: string = '0';
  cap: string = '0';
  children: string = '';
  age: string = ''
  currentAge: string = '';
  tChildren = [];
  childId: number = 0;

  addButton() {
    if (this.value.match(new RegExp(/^\d*[0-9]\d*$/)) && this.type != '') {
      //create a new asset and add it to the assets array
      let asset = { "name": this.name, "type": this.type, "value": this.value, "id": this.counter }
      this.assets = [...this.assets, asset]
      this.counter++;
      //reset the fields
      this.name = ''
      this.type = ''
      this.value = ''
    }
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
    if (this.age != undefined) {
      this.addChildren()
    }
    if(this.match == ''){
      this.match = '0';
    }
    if(this.cap == ''){
      this.cap = '0';
    }
    var arr = this.tChildren.map(x => parseInt(x.age))
    sessionStorage.userData = JSON.stringify(
      {
        "filingStatus": this.filingStatus,
        "income": this.income,
        "retirementDate": this.retirementDate,
        "endOfPlanDate": this.endOfPlan,
        "assets": this.assets,
        "desiredAdditions": this.desiredAdditions,
        "childrensAges": arr,
        "currentAge": this.currentAge,
        "match": this.match,
        "cap": this.cap
      });
  }

  bind() {
    return this.assets
  }

  MakeChildArr(ages) {
    for (var i = 0, childArr = new Array(ages.length); i < ages.length;) {
      childArr[i] = { "age": ages[i], "id": i };
      i++;
    }
    return childArr;
  }

  constructor(private controller: ValidationController) {

    if (sessionStorage.getItem("userData")) {
      var storage = JSON.parse(sessionStorage.userData);
      this.assets = storage.assets
      this.filingStatus = storage.filingStatus
      this.income = storage.income
      this.retirementDate = storage.retirementDate
      this.endOfPlan = storage.endOfPlanDate
      this.desiredAdditions = storage.desiredAdditions
      this.tChildren = this.MakeChildArr(storage.childrensAges)
      this.currentAge = storage.currentAge
      this.cap = storage.cap
      this.match = storage.match
    }
    ValidationRules.customRule(
      'integerRange',
      (value, obj, min, max) => {
        var num = Number.parseInt(value);
        return num === null || num === undefined || (Number.isInteger(num) && num >= min && num <= max);
      },
      "${$displayName} must be an integer between ${$config.min} and ${$config.max}.",
      (min, max) => ({ min, max })
    );

    ValidationRules
      .ensure((m: Home) => m.filingStatus).displayName("Filing Status").required()
      .ensure((m: Home) => m.income).displayName("Income value").required().satisfiesRule('integerRange', 0, 1000000000000)
      .ensure((m: Home) => m.desiredAdditions).displayName("Additions").required().satisfiesRule('integerRange', 0, 1000000000000)
      .ensure((m: Home) => m.currentAge).displayName("Current Age").required().satisfiesRule('integerRange', 1, 200)
      .on(this);
  }

  validateButton() {
    this.jsonify()
    this.controller
      .validate()
      .then(v => {
        if (v.valid){
          window.location.href = "/results"
        }
        else {
          this.message = "You have errors!";
          //document.getElementById("incomeStyle").style.backgroundColor = ('#333')
        }
        v.results.forEach(element =>{
          //Filing Status rule
          if(element.propertyName == 'filingStatus' && !(element.valid))
          {
            document.getElementById("filing").style.borderColor = ('red')
          }
          else if(element.propertyName == 'filingStatus' && (element.valid)){
            document.getElementById("filing").style.borderColor = ('#ccc')
          }
          //end Filing status


          //income status rule
          if(element.propertyName == 'income' && !(element.valid))
          {
            document.getElementById("incomeStyle").style.borderColor = ('red')
          }
          else if(element.propertyName == 'income' && (element.valid)){
            document.getElementById("incomeStyle").style.borderColor = ('#ccc')
          }
          //end income status


           //income addition rule
           if(element.propertyName == 'desiredAdditions' && !(element.valid))
           {
             document.getElementById("additionSytle").style.borderColor = ('red')
           }
           else if(element.propertyName == 'desiredAdditions' && (element.valid)){
             document.getElementById("additionSytle").style.borderColor = ('#ccc')
           }
           //end addition status


            //income age rule
          if(element.propertyName == 'currentAge' && !(element.valid))
          {
            document.getElementById("ageStyle").style.borderColor = ('red')
          }
          else if(element.propertyName == 'currentAge' && (element.valid)){
            document.getElementById("ageStyle").style.borderColor = ('#ccc')
          }
          //end age status


        })
        console.log(v)
        this.errors = v.results;
      })
  }

  // sliders starts here
  attached() {
    var from = new Date().getFullYear() + 1
    var to = from + 80
    var dates = [];
    var range: noUiSlider = <noUiSlider>document.getElementById("range");
    // var Matchtext = document.getElementById("Matchtext")
    // var Ematch = document.getElementById("Ematch")
    // var Ecap = document.getElementById("Ecap")
    // var Captext = document.getElementById("Captext")
    // var typeSelector: HTMLSelectElement;
    var statusSelector: HTMLSelectElement;
    var incomeText = document.getElementById("incomeRange")
    var self = this
    this.retirementDate = from + 20 + ''
    this.endOfPlan = to - 30 + ''

    for (let i = from; i <= to; i += 10) {
      dates.push(i) //get all the years from now for 80 years
    }

    //create slider
    noUiSlider.create(range, {
      start: [from + 20, to - 30],
      range: {
        min: from,
        max: to
      },
      tooltips: true,
      connect: true,
      step: 1,
      format: wNumb({
        decimals: 0
      }),
      pips: {
        mode: 'values',
        values: dates,
        steped: true,
        density: 12
      }
    });

    //save the data when the slider changes value
    range.noUiSlider.on('change', function () {
      self.retirementDate = range.noUiSlider.get()[0]
      self.endOfPlan = range.noUiSlider.get()[1]
    });

    //if they are filing jointly we need combined income
    statusSelector = <HTMLSelectElement>document.getElementById("filing")
    statusSelector.addEventListener("change", function (event) {
      if (this.value == "Joint") {
        incomeText.innerText = "Joint income"
      }
      else {
        incomeText.innerText = "Income"
      }
    })
  }

  addChildren() {
    if (parseInt(this.age) >= 0 && parseInt(this.age) < 18) {
      var child = { "age": this.age, "id": this.childId }
      this.tChildren = [...this.tChildren, child]
      this.childId++;
      this.age = undefined
    }
  }

  removeChild(id) {
    let i = 0;
    this.tChildren.splice(id, 1); //remove the element that you clicked
    this.tChildren.forEach(element => { //fix the array index
      element.id = i
      i++;
    });
    this.childId--;
  }

}


