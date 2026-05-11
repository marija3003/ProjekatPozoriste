export interface PredstavaDTO {
  id: number;
  naziv: string;
  nazivPozorista: string;
  reditelj: string;
  glumci: string[];
  kostimografi: string[];
  putanjaSlike?: string;
}

export interface NovaPredstavaDTO {
  naziv: string;
  pozoristeId: number;
  slika: File; 
}

export interface DodajUcesnikaDTO {
  predstavaId: number;
  zaposleniId: number;
}

export interface DetaljiPredstaveDTO {
    id: number; 
    naziv: string;
    pozoristeId: number;
    nazivPozorista: string;
    ucesnici: string[];
}