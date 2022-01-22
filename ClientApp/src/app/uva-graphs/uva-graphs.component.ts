import { Component, Inject, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
    selector: 'app-uva-graphs',
    templateUrl: './uva-graphs.component.html'
})

export class UvaDatasetComponent {
    public uvas: UVA[] = [];    
    
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string){
        http.get<UVA[]>(baseUrl + "uva").subscribe(result => {
            this.uvas = result;
        }, error => console.error(error));             
    }    
}

export interface UVA {
    date: string,
    value: number,
}