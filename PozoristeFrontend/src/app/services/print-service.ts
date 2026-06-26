import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class PrintService {

printTicket(karta: any, predstava: any, termin: any) {

  const win = window.open('', '_blank');
  if (!win) return;

  win.document.write(`
    <html>
    <body style="font-family: Arial; padding: 20px;">
      <h2>${predstava?.naziv}</h2>

      <p>Kupac: ${karta.imeKupca}</p>
      <p>Kod: TKT-${karta.kartaId}</p>
      <p>Sjedište: ${karta.brojSjedista}</p>
      <p>Termin: ${termin?.datumVrijeme}</p>
    </body>
    </html>
  `);

  win.document.close();
  win.print();
  win.close();
}
}