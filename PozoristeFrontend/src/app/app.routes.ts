import { Routes } from '@angular/router';

import { RepertoarComponent } from './components/repertoar/repertoar.component';
import { ZaposleniComponent } from './components/zaposleni/zaposleni.component';
import { ProdajaComponent } from './components/prodaja/prodaja.component';
import { NovaPredstavaComponent } from './components/nova-predstava/nova-predstava.component';
import { UpravljanjeProdajomComponent } from './components/upravljanje-prodajom/upravljanje-prodajom.component';
import { PozoristaComponent } from './components/pozorista/pozorista/pozorista.component';

export const routes: Routes = [

  {
    path: '',
    component: PozoristaComponent
  },

  
  {
    path: 'theatres/:theatreId/repertoar',
    component: RepertoarComponent
  },
  {
    path: 'theatres/:theatreId/ansambl',
    component: ZaposleniComponent
  },
  {
    path: 'theatres/:theatreId/nova-predstava',
    component: NovaPredstavaComponent
  },
  {
    path: 'theatres/:theatreId/predstave/:id',
    component: ProdajaComponent
  },

  
  {
    path: 'repertoar',
    component: RepertoarComponent
  },
  {
    path: 'zaposleni',
    component: ZaposleniComponent
  },
  {
    path: 'prodaja/:id',
    component: ProdajaComponent
  },
  {
    path: 'nova-predstava',
    component: NovaPredstavaComponent
  },
  {
    path: 'prodane',
    component: UpravljanjeProdajomComponent
  },

  {
    path: '**',
    redirectTo: ''
  }
];