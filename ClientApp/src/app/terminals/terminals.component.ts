import { error } from '@angular/compiler/src/util';
import { Component, OnInit, AfterViewChecked, ViewChild  } from '@angular/core';
import { HttpService } from 'src/app/http.service';
import { NGXLogger } from 'ngx-logger';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';



@Component({
  selector: 'app-terminals',
  templateUrl: './terminals.component.html',
  styleUrls: ['./terminals.component.scss'],
  providers: [HttpService]

})
export class TerminalsComponent implements AfterViewChecked {

  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(public service: HttpService, private logger: NGXLogger) { }

  dataSource = new MatTableDataSource<any>();

  ngAfterViewChecked() {
    this.getTerminalList();
    this.dataSource.sort = this.sort;
  }

 

  getTerminalList() {
    this.service.GetTerminals().subscribe((data: Terminals[]) => {
      this.dataSource = new MatTableDataSource<Terminals>(data);
        this.logger.debug(data);
      error => console.error(error);
    })
  }

  displayedColumns: string[] = ['id', 'name', 'status', 'campaign_Name', 'last_Online', 'address', 'note'];
}

export interface Terminals {
  id: number;
  name: string;
  status: string;
  campaign_Name: string,
  last_Online: string,
  address: string,
  note: string,
  hours_Offline: number,
  sumHours: number
}



