export class Home {
  counter: number = 0;
  assets = [{ "name": "blaj", "type": "401K", "value": "this.value" }];
  name: string
  type: string
  value: string

  regenTable() {
    var assetTable: string =
      "<tr>" +
      `<td>${this.assets[this.counter].name}</td>` +
      `<td>${this.assets[this.counter].type}</td>` +
      `<td>${this.assets[this.counter].value}</td>` +
      "</tr>"

    document.getElementById("AssetTable").innerHTML += assetTable;
    this.counter++;
  }

  addButton() {
    let asset = { "name": this.name, "type": this.type, "value": this.value }
    //JSON.stringify(asset)
    //this.assets.push(asset)
    this.assets = [...this.assets, asset]
    console.log("adding ", this.assets);
    //console.log("Binding "+this.assets);
    //this.regenTable();
  }

  bind() {
    console.log("Binding " + this.assets);
    return this.assets
  }
}
