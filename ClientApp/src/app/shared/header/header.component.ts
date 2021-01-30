import { Component, EventEmitter, Output } from '@angular/core';

@Component({
    selector: 'shared-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
/** header component*/
export class HeaderComponent {

  @Output() toggleMySideBar: EventEmitter<any> = new EventEmitter();
    /** header ctor */
    constructor() {

  }

  toggleSideBar() {
    this.toggleMySideBar.emit();
  }
}
