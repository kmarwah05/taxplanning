import {RouterConfiguration, Router} from 'aurelia-router';
import {PLATFORM} from 'aurelia-pal';
import { Script } from 'vm';
export class App {
  router:Router;
  configureRouter(config, router) {
    config.title = 'Tax Planning';
    config.options.pushState = true;
    config.map([
      {route: ['', 'home'], name: 'home', moduleId: PLATFORM.moduleName('home/home'), nav: true, title: 'Home'},
      {route: 'results', name: 'results', moduleId:  PLATFORM.moduleName('results/results'), nav: true, title: 'Results'},
      {route: 'howitworks', name: 'howitworks', moduleId: PLATFORM.moduleName('howitworks/howitworks'),nav: true, title: 'How It Works'}
    ]);
    this.router = router;
  }

}

