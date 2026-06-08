export enum StanjeKarte {
   Slobodna = "Slobodna", 
   Prodana = "Prodana",
   Stornirana = "Stornirana"
}

export interface TerminDTO {
  id: number;
  datumVrijeme: Date;
  predstavaId: number;
}

export interface KartaDTO {
  id: number;
  brojSjedista: number;
  cijena: number;
  //isProdata: boolean; 
  stanje: StanjeKarte;
}

export interface KupiKartuDTO {
  kartaId: number;
  imeKupca: string;
  //prodavacId: number;
}

export interface NoviTerminDTO {
    predstavaId: number;
    datumVrijeme: Date;
}