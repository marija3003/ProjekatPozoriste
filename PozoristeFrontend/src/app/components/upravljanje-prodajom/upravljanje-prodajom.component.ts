import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { DataService } from '../../services/data.service';
import { TableComponent } from '../../shared/table/table.component';
import { TableColumn } from '../../models/table.model'; 

@Component({
  selector: 'app-upravljanje-prodajom',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    TableComponent
  ],
  templateUrl: './upravljanje-prodajom.component.html',
  styleUrls: ['./upravljanje-prodajom.component.css']
})
export class UpravljanjeProdajomComponent implements OnInit {

  private refresh$ = new BehaviorSubject<void>(undefined);

  prodaneKarte$!: Observable<any[]>;

  odabranaKartaZaStampu: any = null;

  columns: TableColumn[] = [
    {
      key: 'imeKupca',
      label: 'Kupac'
    },
    {
      key: 'predstava',
      label: 'Predstava'
    },
    {
      key: 'brojSjedista',
      label: 'Sjedište'
    },
    {
      key: 'akcije',
      label: 'Akcije',
      type: 'action',
      actions: ['print', 'undo']
    }
  ];

  constructor(
    private dataService: DataService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.prodaneKarte$ = this.refresh$.pipe(
      switchMap(() => this.dataService.getSveProdaneKarte())
    );
  }

  ucitajPodatke(): void {
    this.refresh$.next();
  }

  storniraj(item: any): void {

    if (confirm('Da li želite stornirati ovu prodaju?')) {

      this.dataService.stornirajKartu(item.kartaId).subscribe({

        next: (res) => {

          this.snackBar.open(
            res.message,
            'OK',
            { duration: 3000 }
          );

          this.refresh$.next();
        },

        error: (err) => {

          this.snackBar.open(
            err.error,
            'Zatvori'
          );
        }
      });
    }
  }

  stampaj(karta: any): void {
    
    this.odabranaKartaZaStampu = karta;
    setTimeout(() => {
      window.print();
    }, 200);

  }
}