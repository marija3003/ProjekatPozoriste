import { Component, OnInit, input} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../../services/data.service';
import { ZaposleniDTO } from '../../models/staff.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs'; 
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-nova-predstava',
  templateUrl: './nova-predstava.component.html',
  imports: [ MatIcon, CommonModule, FormsModule],
  styleUrls: ['./nova-predstava.component.css']
})
export class NovaPredstavaComponent implements OnInit {
  currentStep: number = 1;
  theatreId!: number;

  // Form podaci
  naziv: string = '';
  opis: string = '';
  cijena: number = 10;
  predstavaId: number | null = null;
  noviTermin: string = '';
  termini: string[] = [];

  // Liste za selekciju (filtrirane po pozoristu)
  reditelji: ZaposleniDTO[] = [];
  kostimografi: ZaposleniDTO[] = [];
  glumci: ZaposleniDTO[] = [];

  // Selekcije
  selectedDirector: ZaposleniDTO | null = null;
  selectedCostumers: ZaposleniDTO[] = [];
  selectedActors: ZaposleniDTO[] = [];

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private dataService: DataService,
    private snackbar: MatSnackBar
  ) { }

  ngOnInit(): void {
    // 1. Pročitaj ID pozorišta iz URL-a
    this.theatreId = Number(this.route.snapshot.paramMap.get('theatreId'));
    
    // 2. Učitaj zaposlene odmah da budu spremni za korake 2, 3 i 4
    this.ucitajZaposlene();
  }

  ucitajZaposlene() {
    this.dataService.getZaposleniPoPozoristu(this.theatreId).subscribe(res => {
      this.reditelji = res.filter(z => z.tip === 'R');
      this.kostimografi = res.filter(z => z.tip === 'K');
      this.glumci = res.filter(z => z.tip === 'G');
    });
  }

  nextStep() {
    if (this.currentStep === 1) {
      this.sacuvajPredstavuUBazu();
    } else if (this.currentStep === 2 && !this.selectedDirector) {
      this.snackbar.open('Morate odabrati reditelja!', 'Zatvori');
    } else {
      this.currentStep++;
    }
  }

  dodajTermin() {
    if (!this.noviTermin) {
      return;
    }
    this.termini.push(this.noviTermin);
    this.noviTermin = '';
  }

  obrisiTermin(index: number) {
    this.termini.splice(index, 1);
  }
  
  sacuvajPredstavuUBazu() {
    if (!this.naziv) return;

    const formData = new FormData();

    formData.append('Naziv', this.naziv);
    formData.append('PozoristeId', this.theatreId.toString());

    this.termini.forEach((t, index) => {
      formData.append(`Termini[${index}]`, t);
    });

    this.dataService.kreirajPredstavu(formData).subscribe({
      next: (res: any) => {
        this.predstavaId = res.id;
        this.currentStep = 2;
      },
      error: () => this.snackbar.open('Greška pri kreiranju predstave.', 'OK')
    });
  }

  // Selekcija kostimografa (max 2)
  toggleCostumer(z: ZaposleniDTO) {
    const idx = this.selectedCostumers.findIndex(i => i.id === z.id);
    if (idx > -1) {
      this.selectedCostumers.splice(idx, 1);
    } else if (this.selectedCostumers.length < 2) {
      this.selectedCostumers.push(z);
    }
  }

  // Selekcija glumaca (neograničeno)
  toggleActor(z: ZaposleniDTO) {
    const idx = this.selectedActors.findIndex(i => i.id === z.id);
    if (idx > -1) {
      this.selectedActors.splice(idx, 1);
    } else {
      this.selectedActors.push(z);
    }
  }

  // FINALNO: Slanje svih učesnika na backend
 
  finaliziraj() {
  if (!this.predstavaId) {
    this.snackbar.open('Predstava nije kreirana!', 'OK');
    return;
  }

  if (!this.selectedDirector) {
    this.snackbar.open('Odaberite reditelja!', 'OK');
    return;
  }

  if (this.selectedActors.length === 0) {
    this.snackbar.open('Odaberite bar jednog glumca!', 'OK');
    return;
  }

  // 1. PRVO reditelj
  this.dataService.dodajUcesnika({
    predstavaId: this.predstavaId,
    zaposleniId: this.selectedDirector.id
  }).subscribe({
    next: () => {

      // 2. Ostali učesnici (nakon reditelja)
      const ostali: any[] = [];

      // kostimografi
      this.selectedCostumers.forEach(k => {
        ostali.push(
          this.dataService.dodajUcesnika({
            predstavaId: this.predstavaId!,
            zaposleniId: k.id
          })
        );
      });

      // glumci
      this.selectedActors.forEach(g => {
        ostali.push(
          this.dataService.dodajUcesnika({
            predstavaId: this.predstavaId!,
            zaposleniId: g.id
          })
        );
      });

      if (ostali.length === 0) {
        this.finishSuccess();
        return;
      }

      forkJoin(ostali).subscribe({
        next: () => this.finishSuccess(),
        error: () =>
          this.snackbar.open('Greška prilikom dodavanja tima.', 'OK')
      });
    },
    error: () => {
      this.snackbar.open('Greška pri dodavanju reditelja!', 'OK');
    }
  });
}

  private finishSuccess() {
    this.snackbar.open('Umjetnički tim uspješno formiran!', 'OK');
    this.router.navigate(['/theatres', this.theatreId]);
  }
}