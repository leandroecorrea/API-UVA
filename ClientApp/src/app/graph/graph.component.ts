import { Component, Input, OnChanges} from '@angular/core';
import { Chart, registerables } from 'node_modules/chart.js';
import { UvaDatasetComponent, UVA } from '../uva-graphs/uva-graphs.component';
import 'chartjs-adapter-date-fns';

Chart.register(...registerables);


@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css']
})
export class GraphComponent implements OnChanges {
  @Input() lista: UVA[] = [];
  myChart: any;
  
  constructor() {    
  }

  ngOnChanges(): void {    
    if(this.myChart) this.myChart.destroy();
    this.myChart = new Chart("myChart", {      
      type: 'line',
      data: {                
        datasets: [{
                    label: "Precio (en ARS$)",
                    data: this.generateData()                    
                  }]
      },
      options: {          
        scales: {          
          x: {           
              type: 'timeseries',
              time:{
                unit: 'day',

              }
            },
          y: {
            beginAtZero: true
          }
        }        
    }
  }
    );     
  }
  generateData(): Object[] {  
    if(this.lista == undefined) return [];
    return this.lista.map(u => {
      return {
        x: u.date,
        y: u.value
      }      
    });  
  }
}
