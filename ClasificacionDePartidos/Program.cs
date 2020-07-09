using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibreriaSoccer;

namespace ClasificacionDePartidos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Que pais quieres");
            //string pais = Console.ReadLine();
            Season temporada = await SeasonFactory.GetSeasonAsync("mexico");
            
            temporada.resultados();
            /*SoccerTeam local = new SoccerTeam("Tigres UANL",0);
            SoccerTeam visitant = new SoccerTeam("Club Tijuana",0);
            GameLive gamelive = new GameLive(local,visitant);
            gamelive.updateGame+=temporada.onUpdateGame;
            gamelive.VisitantScore();
            gamelive.LocalScore();
            gamelive.LocalScore();
            gamelive.finishGame();
            gamelive.VisitantScore();
            */
            //await temporada.ReadSeasonFromFile();
            
            //temporada.resultados();
            
            
            //Console.ReadKey();
              
                            
        }
    }
}
