export class Home{
counter:number= 0;
assets = [];
name: string
type: string
value: string

  regenTable(){
   var assetTable: string = "<tr>"+
   "<td><input type=\"text\" class=\"form-control\" id=\"Aname\" value.bind=\"name\"></td>"+
   "<td><select class=\"form-control h-100\" id=\"Atype\" value.bind=\"type\">"+
       "<option>Choose Asset Type</option>"+
       "<option value=\"Ira\">IRA</option>"+
       "<option value=\"RothIra\">Roth IRA</option>"+
       "<option value=\"401K\">401K</option>"+
   "</select></td>"+
   "<td><input type=\"text\" class=\"form-control\" id=\"Avalue\" value.bind=\"value\"></td>"+
   `<th><button click.delegate=\"addButton()\"><span class=\"glyphicon glyphicon-plus addBtn\"></th>`+
 "</tr>"+
 "<tr repeat.for=\"assets of $displayData\">"+
   `<td>${this.assets[this.counter].name}</td>`+
   `<td>${this.assets[this.counter].type}</td>`+
   `<td>${this.assets[this.counter].value}</td>`+
 "</tr>"

   document.getElementById("AssetTable").innerHTML += "";
   this.counter++;
  }

  addButton(){
    let asset = {"name":this.name,"type":this.type, "value":this.value}
    JSON.stringify(asset)
    this.assets.push(asset)
    //console.log("Binding "+this.assets);
    this.regenTable();
    this.counter++;
  }

  bind(){
    console.log("Binding "+this.assets);
    return this.assets
  }
}
