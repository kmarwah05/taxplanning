import {RouterConfiguration, Router} from 'aurelia-router';

export class App {
  router:Router;
  configureRouter(config, router) {
    config.title = 'Aurelia';
    config.map([
      {route: ['', 'home'], name: 'home', moduleId: 'Home/home', nav: true, title: 'Home'},
      {route: 'results', name: 'results', moduleId: 'Results/results', nav: true, title: 'Results'}
    ]);
    this.router = router;
  }
}
