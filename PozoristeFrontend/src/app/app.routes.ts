import { Routes } from '@angular/router';
import { RepertoarComponent } from './components/repertoar/repertoar.component';
import { ZaposleniComponent } from './components/zaposleni/zaposleni.component';
import { ProdajaComponent } from './components/prodaja/prodaja.component';
import { NovaPredstavaComponent } from './components/nova-predstava/nova-predstava.component';

export const routes: Routes = [
  { path: 'repertoar', component: RepertoarComponent },
  { path: 'zaposleni', component: ZaposleniComponent },
  { path: 'prodaja/:id', component: ProdajaComponent},
  { path: 'nova-predstava', component: NovaPredstavaComponent},
  { path: '', redirectTo: 'repertoar', pathMatch: 'full' },
  { path: '**', redirectTo: 'repertoar' } 
];