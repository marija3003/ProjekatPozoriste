import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { ZaposleniDTO, NoviZaposleniDTO } from '../../models/staff.model';
import { PozoristeDTO } from '../../models/theater.model';
import { MatTable, MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSidenavModule } from '@angular/material/sidenav';

@Component({
  selector: 'app-zaposleni',
  imports: [ CommonModule, FormsModule,  
             MatFormFieldModule, MatInputModule, 
             MatSelectModule, MatButtonModule,
             MatCardModule, MatIconModule, 
             MatChipsModule, MatTableModule , MatSidenavModule ],
  templateUrl: './zaposleni.component.html',
  styleUrls: ['./zaposleni.component.css']
})
export class ZaposleniComponent implements OnInit {
  @ViewChild(MatTable) table?: MatTable<ZaposleniDTO>;

  dataSource = new MatTableDataSource<ZaposleniDTO>([]);
  pozorista: PozoristeDTO[] = [];

  noviZaposleni: NoviZaposleniDTO = {
    ime: '',
    prezime: '',
    tip: 'G',
    pozoristeId: 0
  };

  displayedColumns: string[] = ['imePrezime', 'tip', 'nazivPozorista'];

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
 
    this.ucitajPodatke();
  }

  ucitajPodatke(){
    this.dataService.getZaposleni().subscribe(res => {
      this.dataSource.data = res;
      this.table?.renderRows();
    });
    this.dataService.getPozorista().subscribe(res => this.pozorista = res);
  }

    dodajZaposlenog() {
    if (!this.noviZaposleni.ime || !this.noviZaposleni.prezime || this.noviZaposleni.pozoristeId === 0) {
      alert("Molimo popunite sva polja!");
      return;
    }

    this.dataService.kreirajZaposlenog(this.noviZaposleni).subscribe({
      next: () => {
        this.dataService.getZaposleni().subscribe(res => {
          this.dataSource.data = res;
          this.table?.renderRows();
        });
        this.noviZaposleni = { ime: '', prezime: '', tip: 'G', pozoristeId: 0 };
      },
      error: (err) => console.error("Greška pri dodavanju:", err)
    });
  }

  getTipNaziv(tip: string){
    if ( tip === 'G') return 'Glumac';
    if ( tip === 'R') return 'Reditelj';
    if ( tip === 'K') return 'Kostimograf';
 
    return tip;
  }

}
