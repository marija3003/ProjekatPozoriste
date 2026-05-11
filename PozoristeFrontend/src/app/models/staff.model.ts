export enum TipZaposlenog {
  Glumac = 'G',
  Reditelj = 'R',
  Kostimograf = 'K'
}

export interface ZaposleniDTO {
  id: number;
  imePrezime: string;
  tip: string;
  pozoristeId: number;
  nazivPozorista: string;
}

export interface NoviZaposleniDTO {
  ime: string;
  prezime: string;
  tip: string; 
  pozoristeId: number;
}