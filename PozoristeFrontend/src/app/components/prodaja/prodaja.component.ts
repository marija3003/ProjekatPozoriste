import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { ChangeDetectorRef } from '@angular/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { TerminDTO, KupiKartuDTO, KartaDTO, StanjeKarte } from '../../models/ticketing.model';
import { PredstavaDTO } from '../../models/show.model';

@Component({
  selector: 'app-prodaja',
  imports: [CommonModule, FormsModule, 
            MatButtonModule, MatSelectModule, 
            MatInputModule, MatCardModule,
            MatIconModule, MatSnackBarModule],
  templateUrl: './prodaja.component.html',
  styleUrls: ['./prodaja.component.css']
})
export class ProdajaComponent implements OnInit {
  predstavaId!: number;
  predstava: PredstavaDTO | null = null;
  termini: TerminDTO[] = [];
  selectedTerminId: number | null = null;
  karte: KartaDTO[] = [];
  prodaneKarte: any[] = []; 
  selectedKarta: KartaDTO | null = null;
  imeKupca: string = '';
  readonly SEATS_PER_ROW = 12;

  constructor(
    private route: ActivatedRoute, 
    private dataService: DataService,
    private snackBar: MatSnackBar, 
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.predstavaId = Number(this.route.snapshot.paramMap.get('id'));
    this.ucitajDetaljePredstave();
    this.ucitajTermine();
  }

  ucitajDetaljePredstave() {
    this.dataService.getRepertoar().subscribe(res => {
      this.predstava = res.find(p => p.id === this.predstavaId) ?? null;
    });
  }

  ucitajTermine() {
    this.dataService.getTerminiZaPredstavu(this.predstavaId).subscribe(res => {
      this.termini = res.filter(t => !this.isTerminInPast(t));

      if (this.termini.length > 0) {
        this.selectedTerminId = this.termini[0].id;
        this.onTerminChange();
      }
    });
  }

  onTerminChange() {
    if (this.selectedTerminId) {
      this.ucitajSjedista();
      this.ucitajProdaneKarte(); 
      this.selectedKarta = null;
    }
  }

  ucitajSjedista() { 
    this.dataService.getKarteZaTermin(this.selectedTerminId!).subscribe(res => {
      this.karte = res;
      this.cdr.detectChanges();
    });
  }

  ucitajProdaneKarte() {
    this.dataService.getSveProdaneKarte().subscribe(res => {
      const kartaIds = this.karte.map(k => k.id);
      this.prodaneKarte = res.filter(p => kartaIds.includes(p.kartaId));
    });
  }
  
  getRowLabel(index: number): string {
    return String.fromCharCode(65 + index); 
  }
  
  odaberiSjediste(karta: KartaDTO) {
    if (!this.isSlobodno(karta)) return;
    this.selectedKarta = karta;
  }

  getFullSjedisteLabel(broj: number): string {
      const rowIndex = Math.floor((broj - 1) / this.SEATS_PER_ROW);
      const seatInRow = ((broj - 1) % this.SEATS_PER_ROW) + 1;
      const rowLabel = String.fromCharCode(65 + rowIndex); // 0 -> A, 1 -> B...
      return `Red ${rowLabel}, Sjedište ${seatInRow}`;
  }

  isSlobodno(karta: KartaDTO): boolean {
    return karta.stanje === StanjeKarte.Slobodna;
  }

  get sjedistaPoRedovima() {
    const rows = [];
    for (let i = 0; i < this.karte.length; i += this.SEATS_PER_ROW) {
      rows.push(this.karte.slice(i, i + this.SEATS_PER_ROW));
    }
    return rows;
  }

   potvrdiKupovinu() {
    if(!this.imeKupca || !this.selectedKarta) return;

    const dto: KupiKartuDTO = {
      kartaId: this.selectedKarta.id,
      imeKupca: this.imeKupca,
      // prodavacId: 1
    }

    this.dataService.prodajKartu(dto).subscribe({
      next: () => {
        this.snackBar.open('Karta prodata!', 'OK', {duration: 2000});
        this.ucitajSjedista();
        this.ucitajProdaneKarte();
        this.selectedKarta = null;
        this.imeKupca = '';
      }
    }); 
  }

     
  private isTerminInPast(termin: any) {
    return new Date(termin.datumVrijeme).getTime() < Date.now();
  }
  
  idiNazad() { this.router.navigate(['/repertoar']); }

  getStanjeKarte(kartaId: number): string {
    const karta = this.karte.find(k => k.id === kartaId);
    return karta?.stanje ?? '';
  }

  
  storniraj(kartaId: number): void {
    if (confirm('Da li želite stornirati ovu prodaju?')) {
      this.dataService.stornirajKartu(kartaId).subscribe({
        next: (res) => {
          this.snackBar.open(res.message, 'OK', {
            duration: 3000
          });

          this.ucitajSjedista();
          this.ucitajProdaneKarte();
          
        },

        error: (err) => {
          this.snackBar.open(err.error, 'Zatvori'); 
        }
      });
    }
  }

  stampaj(karta: any): void {
    const content = `
      <html>
        <head>
          <title>Karta</title>
          <style>
            body {
              font-family: Arial, sans-serif;
              padding: 30px;
            }

            .ticket {
              border: 2px solid black;
              padding: 20px;
              max-width: 500px;
            }

            h2 {
              margin-top: 0;
            }

            .row {
              margin: 10px 0;
            }
          </style>
        </head>
        <body>
          <div class="ticket">
            <h2>${this.predstava?.naziv}</h2>

            <div class="row">
              <strong>Kupac:</strong> ${karta.imeKupca}
            </div>

            <div class="row">
              <strong>Karta:</strong> TKT-${karta.kartaId}
            </div>

            <div class="row">
              <strong>Sjedište:</strong> ${this.getFullSjedisteLabel(karta.brojSjedista)}
            </div>

            <div class="row">
              <strong>Datum:</strong>
              ${
                this.termini.find(t => t.id === this.selectedTerminId)
                  ?.datumVrijeme ?? ''
              }
            </div>
          </div>
        </body>
      </html>
    `;

    const printWindow = window.open('', '_blank');

    if (printWindow) {
      printWindow.document.write(content);
      printWindow.document.close();

      printWindow.onload = () => {
        printWindow.print();
        printWindow.close();
      };
    }
  }
}