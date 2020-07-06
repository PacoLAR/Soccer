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
            
                    
        }
    }
}
