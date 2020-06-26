using System;
using System.Collections.Generic;
using LibreriaSoccer;

namespace ClasificacionDePartidos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Que pais quieres");
            string pais = Console.ReadLine();
            Season temporada = SeasonFactory.GetSeason(pais);
            temporada.resultados();
            Console.ReadKey();
            //Season temporada = new Season();
            //ITableResults resultados = new TablaDeResultados();
            //List<SoccerTeam> listaequipos = temporada.ReadSeasonFromFile("es.1.csv");           
            //resultados.mostrarResultados(listaequipos);
            //ITableResults nuevo = new CrearArchivoConTemporada();
            //nuevo.mostrarResultados(listaequipos);         
        }
    }
}
