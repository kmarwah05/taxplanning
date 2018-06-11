export class Home {
  counter: number = 0;
  assets = [];
  name: string
  type: string
  value: string

  addButton() {
    let asset = { "name": this.name, "type": this.type, "value": this.value, "id":this.counter }
    this.assets = [...this.assets, asset]
    this.counter++;
    console.log("adding ", this.assets);
  }

  removeButton(id){
    let i = 0;
    console.log("Removing",id);
    this.assets.splice(id,1);
    this.assets.forEach(element => {
      element.id = i
      i++;
    });
  }

  bind() {
    console.log("Binding " + this.assets);
    return this.assets
  }
}
