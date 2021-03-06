using System;

namespace LibreriaSoccer{
    public class SeasonFactory{
        public static async System.Threading.Tasks.Task<Season> GetSeasonAsync(string country){
            Season temporada;
            string pais = country.ToUpper();
            
            if(pais=="MEXICO"){
                ITableResults mexico = new TablaDeResultados();
                temporada = new Season(mexico,"mx.1.csv");
                await temporada.ReadSeasonFromFile();
                               
            }else{
                ITableResults resultados = new CrearArchivoConTemporada();
                if(pais=="ENGLAND"){
                    temporada = new Season(resultados,"eng.1.csv");
                    await temporada.ReadSeasonFromFile();
                }else{
                    temporada = new Season(resultados,"es.1.csv");
                    await temporada.ReadSeasonFromFile();
                }
            }
            return temporada;
        }
    }
}