import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { PozoristeDTO } from '../../models/theater.model';
import { ZaposleniDTO } from '../../models/staff.model';
import { DodajUcesnikaDTO } from '../../models/show.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-nova-predstava',
  imports: [ CommonModule, FormsModule, 
             MatFormFieldModule, MatInputModule, 
             MatSelectModule, MatButtonModule, 
             MatCardModule, MatIconModule,
             MatListModule, MatSnackBarModule ],
  templateUrl: './nova-predstava.component.html',
  styleUrls: ['./nova-predstava.component.css']
})
export class NovaPredstavaComponent implements OnInit {

  naziv: string = '';
  pozoristeId: number = 0;
  odabranaSlika: File | null = null;
  slikaPreview: string | null = null;

  predstavaId: number | null = null; 
  pozorista: PozoristeDTO[] = [];
  dostupniZaposleni: ZaposleniDTO[] = [];
  dodatiUcesnici: ZaposleniDTO[] = [];

  constructor(private dataService: DataService, private snackbar: MatSnackBar) { }
  
  ngOnInit() : void {
    this.dataService.getPozorista().subscribe(
      res => this.pozorista = res
    );
  }

  onFileSelected(event: any){
    this.odabranaSlika = event.target.files[0];
    if (this.odabranaSlika){
      const reader = new FileReader();
      reader.onload = (e: any) => this.slikaPreview = e.target.result;
      reader.readAsDataURL(this.odabranaSlika);
    }
  } 

  kreirajOsnovno(){
    if (!this.naziv || this.pozoristeId === 0 || !this.odabranaSlika){
      this.snackbar.open('Popunite sva polja i dodajte sliku!', 'OK');
      return;
    }

    const formData = new FormData();
    formData.append('naziv', this.naziv);
    formData.append('pozoristeId', this.pozoristeId.toString());
    formData.append('slika', this.odabranaSlika);

    this.dataService.kreirajPredstavu(formData).subscribe({
      next: (res: any) => {
        this.predstavaId = res.id;
        this.snackbar.open('Predstava kreirana! Sada dodajte tim.', 'OK');
        this.ucitajZaposleneZaPozoriste();
      }, 
      error: (err) => this.snackbar.open('Greška pri kreiranju', 'Zatvori')
    });
  }

  ucitajZaposleneZaPozoriste(){
    this.dataService.getZaposleniPoPozoristu(this.pozoristeId).subscribe(
      res=> {
        this.dostupniZaposleni = res;
      }
    );
  }

  dodajUcesnika(z: ZaposleniDTO){
    if (z.tip === 'R' && this.dodatiUcesnici.some( u => u.tip === 'R')) {
      this.snackbar.open('Predstava može imati samo jednog reditelja!', 'Zatvori');
      return;
    }

    if (z.tip === 'K' && this.dodatiUcesnici.filter(u => u.tip === 'K').length >= 2){
      this.snackbar.open('Dozvoljena su maksimalno dva kostimografa!', 'Zatvori');
      return;
    }

    if(this.dodatiUcesnici.some(u => u.id === z.id)){
      this.snackbar.open("Ovaj umjetnik je već u timu!", 'Zatvori');
      return;
    }

    const dto: DodajUcesnikaDTO = {
      predstavaId: this.predstavaId!,
      zaposleniId: z.id
    };

    this.dataService.dodajUcesnika(dto).subscribe({
      next: () => {
        this.dodatiUcesnici.push(z);
        this.snackbar.open (`$(z.imePrezime) dodan u tim!`, 'OK', {duration: 2000});
      },
      error: (err) => this.snackbar.open(err.error || 'Greška', 'Zatvori')
    });  
   
  }

  getTipLabel(tip: string){
    if (tip === 'G') return 'Glumac';
    if (tip === 'K') return 'Kostimograf';
    if (tip === 'R') return 'Reditelj';
    return '';
  }
}
