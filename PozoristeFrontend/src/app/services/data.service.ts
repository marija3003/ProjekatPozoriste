import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
 
import { PozoristeDTO, NovoPozoristeDTO } from "../models/theater.model";
import { ZaposleniDTO, NoviZaposleniDTO } from "../models/staff.model";
import { PredstavaDTO, DodajUcesnikaDTO } from "../models/show.model";
import { KartaDTO, KupiKartuDTO, TerminDTO, NoviTerminDTO, StanjeKarte } from "../models/ticketing.model";

@Injectable({
    providedIn: 'root'
})
export class DataService {
    private readonly apiUrl = 'https://localhost:7050/api';

    constructor(private http: HttpClient){}


    //POZORISTE

    getPozorista(): Observable<PozoristeDTO[]> {
        return this.http.get<PozoristeDTO[]>(`${this.apiUrl}/Pozoriste`);
    }

    kreirajPozoriste(dto: NovoPozoristeDTO): Observable<any>{
        return this.http.post(`${this.apiUrl}/Pozoriste`, dto);
    }

    //ZAPOSLENI
    getZaposleni(): Observable<ZaposleniDTO[]>{
        return this.http.get<ZaposleniDTO[]>(`${this.apiUrl}/Zaposleni`);
    }

    getZaposleniPoPozoristu(pozoristeId: number): Observable<ZaposleniDTO[]>{
        return this.http.get<ZaposleniDTO[]>
        (`${this.apiUrl}/Zaposleni/Pozoriste/${pozoristeId}`);
    }

    getZaposleniPoTipu(tip: string): Observable<ZaposleniDTO[]>{
        return this.http.get<ZaposleniDTO[]>
        (`${this.apiUrl}/Zaposleni/Filter/${tip}`);
    }

    kreirajZaposlenog(dto: NoviZaposleniDTO): Observable<any>{
        return this.http.post(`${this.apiUrl}/Zaposleni`, dto);
    }

    updateZaposlenog(id: number, dto: NoviZaposleniDTO): Observable<any> {
        return this.http.put(`${this.apiUrl}/Zaposleni/${id}`, dto);
    }

    obrisiZaposlenog(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Zaposleni/${id}`);
    }

    //PREDSTAVE
    getRepertoar(): Observable<PredstavaDTO[]>{
        return this.http.get<PredstavaDTO[]>
        (`${this.apiUrl}/Predstava`);
    }

    kreirajPredstavu(formData:FormData): Observable<any>{
        return this.http.post(`${this.apiUrl}/Predstava`, formData);
    }

    dodajUcesnika(dto: DodajUcesnikaDTO): Observable<any>{
        return this.http.post(`${this.apiUrl}/Predstava/dodaj-ucesnika`, dto);
    }

    obrisiPredstavu(id: number){
        return this.http.delete(`${this.apiUrl}/Predstava/${id}`);
    }

    //TERMINI I KARTE
    getTerminiZaPredstavu(predstavaId: number): Observable<any[]>{
        return this.http.get<any[]>
        (`${this.apiUrl}/Predstava/${predstavaId}/termini`);
    }

    dodajTermin(dto: NoviTerminDTO): Observable<any> {
        return this.http.post(`${this.apiUrl}/Predstava/termin`, dto);
    }

    getKarteZaTermin(terminId: number): Observable<KartaDTO[]> {
        return this.http.get<KartaDTO[]>
        (`${this.apiUrl}/Predstava/${terminId}/karte`)
        .pipe(
            map(karte => karte.map(karta => this.normalizeKartaState(karta)))
        );
    }

    private normalizeKartaState(karta: any): KartaDTO {
        if (karta == null) {
            return { id: 0, brojSjedista: 0, cijena: 0, stanje: StanjeKarte.Slobodna };
        }

        let stanje = karta.stanje;
        if (stanje === undefined || stanje === null || stanje === '') {
            return { ...karta, stanje: StanjeKarte.Slobodna };
        }

        if (typeof stanje === 'number') {
            const values = [StanjeKarte.Slobodna, StanjeKarte.Prodana, StanjeKarte.Stornirana];
            return { ...karta, stanje: values[stanje] ?? StanjeKarte.Slobodna };
        }

        const normalized = String(stanje).trim();
        if (Object.values(StanjeKarte).includes(normalized as StanjeKarte)) {
            return { ...karta, stanje: normalized as StanjeKarte };
        }

        return { ...karta, stanje: StanjeKarte.Slobodna };
    }

    //PRODAJA I STORNIRANJE
    prodajKartu(dto:KupiKartuDTO): Observable<any>{
        return this.http.post(`${this.apiUrl}/Karta/prodaj`, dto);
    }

    stornirajKartu(kartaId: number): Observable<any> {
        return this.http.post(`${this.apiUrl}/Karta/storniraj/${kartaId}`, {});
    }

    getSveProdaneKarte(): Observable<any[]>{
        return this.http.get<any[]>(`${this.apiUrl}/Karta/prodane`);
    }
    
}