import { Component } from '@angular/core';

@Component({
    selector: 'layout-default',
    templateUrl: './default.component.html',
    styleUrls: ['./default.component.scss']
})
/** default component*/
export class DefaultComponent {

  sideBarOpen = true;
    /** default ctor */
    constructor() {

  }

  sideBarToggler() {
    this.sideBarOpen = !this.sideBarOpen;
  }
}
