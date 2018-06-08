export class App {

counter:number= 0;
 
  addButton(){
   var assetTable: string = "<tr><td><input type=\"text\" id=\"AssetName"+this.counter+"\" class=\"form-control\"/></td>"

   assetTable += 
   

   "<td><input type=\"text\" id=\"AssetType"+this.counter+"\"class=\"form-control\"/></td>"+
   "<td><input type=\"text\" id=\"AssetValue"+this.counter+"\" class=\"form-control\" /></td>"+
   "</tr>"

   document.getElementById("assetIdTable").innerHTML += (assetTable);
   this.counter++;
  }



  removeButton(){
    var assetTable2: string = "<tr><td><input type=\"text\" id=\"AssetName"+this.counter+"\" class=\"form-control\"/></td>"


    "<td><input type=\"text\" id=\"AssetType"+this.counter+"\"class=\"form-control\"/></td>"

    //document.getElementById("assetIdTable").innerHTML-=assetTable2;
    this.counter--;


  }
}
