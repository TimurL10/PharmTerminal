import { AfterViewChecked, Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { HttpParams, HttpClient } from "@angular/common/http";
import { HttpService } from 'src/app/http.service';
import { ReportByNet } from 'src/app/reportbynet';
import { NGXLogger } from 'ngx-logger';
import { AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatTable } from '@angular/material/table';
import * as Highcharts from 'highcharts';
import HC_exporting from 'highcharts/modules/exporting';
import { FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';




@Component({
    selector: 'widget-area',
    templateUrl: './area.component.html',
  styleUrls: ['./area.component.scss'],
  providers: [HttpService]
})



/** area component*/
export class AreaComponent {
  value = 'Clear me';
  guid: string;
  Highcharts = Highcharts;
  chartOptions: {} = [];
  report: ReportByNet[];
  dataSource = new MatTableDataSource<any>();
  displayedColumns: string[] = ['storeName', 'ordersCount', 'soldOrders',
    'timeOutCanceled', 'customerCanceledOrders', 'noEndStatusOrd', 'canceledOrd', 'noReceiveStatusOrd'];
  start: string;
  end: string;
  params: HttpParams;
  range = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  ngOnInit() {
    this.chartOptions = {
      chart: {
        type: 'area'
      },
      title: {
        text: 'Area chart with values'
      },
      xAxis: {
        categories: ['Apples', 'Oranges', 'Pears']
      },
      credits: {
        enabled: false
      },
      exporting: {
        enabled: true,
      },
      series: [{
        name: 'John',
        data: [5, 3, 4]
      }, {
        name: 'Jane',
        data: [2, -2, -2]
      }, {
        name: 'Joe',
        data: [3, 4, 4]
        }, {
          name: 'Gigi',
          data: [2, -1, -2]
        }]
    };

    HC_exporting(Highcharts);

    setTimeout(() => {
      window.dispatchEvent(
        new Event('resize')
      );
    }, 300);
  }

  //@ViewChild(MatSort) sort: MatSort;


  ngAfterViewChecked() {
   // this.dataSource.sort = this.sort;
  }

  /** StoreReport ctor */
  constructor(private httpService: HttpService, private http: HttpClient, private logger: NGXLogger) {

  }

  makeFloor(num: number) {
    return Math.floor(num);
  }

  loadReport() {
    if (this.range.value.start == null || this.range.value.end == null) {

    }
    this.start = this.range.value.start;
    this.end = this.range.value.end;
    this.params = new HttpParams().set('start', this.start).set('end', this.end);

    this.httpService.GetReportForStores(this.params)
      .subscribe((data: ReportByNet[]) => {
        this.report = data,
          this.dataSource = new MatTableDataSource<ReportByNet>(data);
        error => console.error(error)
      });
  }

  loadChart() {
    this.logger.debug('hi' + this.guid);
    
  }
}


export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
  symbol2: string;

}
