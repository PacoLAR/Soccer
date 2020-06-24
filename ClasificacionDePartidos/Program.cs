using System;
using System.Collections.Generic;
using LibreriaSoccer;

namespace ClasificacionDePartidos
{
    class Program
    {
        static void Main(string[] args)
        {
            Season temporada = new Season();
            ITableResults resultados = new TablaDeResultados();
            List<SoccerTeam> listaequipos = temporada.ReadSeasonFromFile("es.1.csv");           
            resultados.mostrarResultados(listaequipos);
            ITableResults nuevo = new CrearArchivoConTemporada();
            //nuevo.mostrarResultados(listaequipos);         
        }
    }
}
