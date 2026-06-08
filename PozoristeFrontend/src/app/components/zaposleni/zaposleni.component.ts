import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { BehaviorSubject, Observable, switchMap } from 'rxjs';

import { DataService } from '../../services/data.service';

import {
  ZaposleniDTO,
  NoviZaposleniDTO
} from '../../models/staff.model';

import { PozoristeDTO } from '../../models/theater.model';
import { TableColumn } from '../../models/table.model';

import { TableComponent } from '../../shared/table/table.component';

import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-zaposleni',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule,
    MatTableModule,
    MatSidenavModule,
    TableComponent
  ],
  templateUrl: './zaposleni.component.html',
  styleUrls: ['./zaposleni.component.css']
})
export class ZaposleniComponent implements OnInit {

  @ViewChild('drawer') drawer!: MatSidenav;

  isEditMode: boolean = false;
  selectedId: number | null = null;

  private refresh$ = new BehaviorSubject<void>(undefined);

  zaposleni$!: Observable<ZaposleniDTO[]>;
  pozorista$!: Observable<PozoristeDTO[]>;

  noviZaposleni: NoviZaposleniDTO = {
    ime: '',
    prezime: '',
    tip: 'G',
    pozoristeId: 0
  };

  cols: TableColumn[] = [
    {
      key: 'imePrezime',
      label: 'Ime i Prezime',
      type: 'text'
    },
    {
      key: 'tip',
      label: 'Uloga',
      type: 'chip',
      valueMap: {
        G: { label: 'Glumac', class: 'chip-g' },
        R: { label: 'Reditelj', class: 'chip-r' },
        K: { label: 'Kostimograf', class: 'chip-k' }
      }
    },
    {
      key: 'nazivPozorista',
      label: 'Pozorište',
      type: 'text'
    },
    {
      key: 'actions',
      label: 'Akcije',
      type: 'action',
      actions: ['edit', 'delete']
    }
  ];

  constructor(
    private dataService: DataService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {

    this.zaposleni$ = this.refresh$.pipe(
      switchMap(() => this.dataService.getZaposleni())
    );

    this.pozorista$ = this.dataService.getPozorista();
   }

  otvoriZaDodavanje(): void {

    this.isEditMode = false;
    this.selectedId = null;

    this.noviZaposleni = {
      ime: '',
      prezime: '',
      tip: 'G',
      pozoristeId: 0
    };

    this.drawer.open();
    
  }

  handleDelete(z: ZaposleniDTO): void {

    if (confirm(`Da li ste sigurni da želite obrisati ${z.imePrezime}?`)) {

      this.dataService.obrisiZaposlenog(z.id)
        .subscribe(() => {

          this.refresh$.next();

          this.snackBar.open(
            'Zaposleni uspješno obrisan',
            'OK',
            { duration: 3000 }
          );
        });
    }
  }

  handleEdit(z: ZaposleniDTO): void {

    this.isEditMode = true;
    this.selectedId = z.id;

    const splitImenaIPrezimena = z.imePrezime.split(' ');

    this.noviZaposleni = {
      ime: splitImenaIPrezimena[0],
      prezime: splitImenaIPrezimena.slice(1).join(' '),
      tip: z.tip,
      pozoristeId: z.pozoristeId
    };

    this.drawer.open();
  }

  sacuvaj(): void {

    if (this.isEditMode && this.selectedId) {

      this.dataService
        .updateZaposlenog(this.selectedId, this.noviZaposleni)
        .subscribe(() => {

          this.zavrsiAkciju(
            'Podaci uspješno izmijenjeni'
          );
        });

    } else {

      this.dataService
        .kreirajZaposlenog(this.noviZaposleni)
        .subscribe(() => {

          this.zavrsiAkciju(
            'Zaposleni uspješno dodan'
          );
        });
    }
  }

  zavrsiAkciju(poruka: string): void {

    this.snackBar.open(
      poruka,
      'OK',
      { duration: 3000 }
      
    );

    this.refresh$.next();
    this.drawer.close();
  }

  getTipNaziv(tip: string): string {

    if (tip === 'G') return 'Glumac';
    if (tip === 'R') return 'Reditelj';
    if (tip === 'K') return 'Kostimograf';
    
    return tip;
  }
}