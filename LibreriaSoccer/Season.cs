using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibreriaSoccer{
    public class Season{  
        public Season(ITableResults TableResults,string FileLocation){
            this.TableResults = TableResults;
            this.FileLocation = FileLocation;
            Teams = new List<SoccerTeam>();
            Games = new List<Game>();           
        }

        ITableResults TableResults{get;set;}
        string FileLocation{get;set;}
        public List<SoccerTeam> Teams{get;private set;}
        public List<Game> Games {get;private set;}

       
        public async Task<List<SoccerTeam>> ReadSeasonFromFile(){
            string patron = ("\\.csv$");
            Regex  nuevo = new Regex(patron);
            MatchCollection encontro = nuevo.Matches(FileLocation);                      
            if(encontro.Count>0){            
                try{
                   
                    string [] lineas = await File.ReadAllLinesAsync(FileLocation);
        
                    Games = saveGames(lineas);            
                    Teams = llenarClasificacion(Games);                   
                }catch (DirectoryNotFoundException ){
                    Console.WriteLine("No encontré el directorio");
                }catch (FileNotFoundException ){
                    Console.WriteLine("No encontré el archivo");
                }
                catch (IOException ){
                    Console.WriteLine("Error al leer el archivo");
                }               
                return Teams;
            }else{
                Console.WriteLine("Solo archivos con formato csv");
                return null;
            }
            
        }
        
        public List<SoccerTeam> llenarClasificacion(List<Game> Games){
            var nombres = (from game in Games  select  (game.Local.Equipo)).Distinct().ToList();
            foreach (var nombre in nombres)
            {
            short puntos = Convert.ToInt16(Games.Sum(c=> (c.FullTimeResult == ResultadosPartida.LocalWon && c.Local.Equipo == nombre)?3
                                :(c.FullTimeResult == ResultadosPartida.VisitantWon && c.Visitant.Equipo == nombre)?3
                                :(c.FullTimeResult == ResultadosPartida.Draw && (c.Local.Equipo == nombre || c.Visitant.Equipo == nombre))?1:0));

            int golesMarcados = Games.Sum(c =>(c.Local.Equipo == nombre)? c.GoalsLocal
                                :(c.Visitant.Equipo ==nombre)? c.GoalsVisitant
                                :0);

            int golesRecibidos = Games.Sum(c =>(c.Local.Equipo == nombre)? c.GoalsVisitant
                                :(c.Visitant.Equipo ==nombre)? c.GoalsVisitant
                                :0);                     

            
            SoccerTeam equipo = new SoccerTeam(nombre,puntos);
            equipo.GoalsScored = golesMarcados;
            equipo.GoalsRecived = golesRecibidos;
            if(!Teams.Exists(c => c.Equipo == equipo.Equipo)){
                Teams.Add(equipo);
            }else{
                SoccerTeam equipoencontrado= Teams.Find(c => c.Equipo == equipo.Equipo);
                equipoencontrado.Puntos = Convert.ToInt16(puntos);
            }
            clasificar();
            Teams.Reverse();
            }
            return Teams;                              
        }
        public List<Game> GetGames(string localTeam =null,string visitantTeam=null,string date = null ){
           
            var listajuegos = from g in Games 
                    where ((g.Local.Equipo == localTeam || String.IsNullOrEmpty(localTeam))
                     && (g.Visitant.Equipo == visitantTeam || String.IsNullOrEmpty(visitantTeam))
                     && (g.fecha == Convert.ToDateTime(date) || String.IsNullOrEmpty(date)))
                    select g;
            return listajuegos.ToList();            
        }
        
        public ResultadosPartida determinarPartido(string resultado,Game partido){
            string [] golesAnotados = resultado.Split('-');
            short golesEquipoLocal = Convert.ToInt16(golesAnotados[0]);
            short golesEquipoVisitante = Convert.ToInt16(golesAnotados[1]);
            partido.GoalsLocal = golesEquipoLocal;
            partido.GoalsVisitant = golesEquipoVisitante;
            
            return  (golesEquipoLocal>golesEquipoVisitante) ?ResultadosPartida.LocalWon
            :(golesEquipoLocal<golesEquipoVisitante)? ResultadosPartida.VisitantWon
            :ResultadosPartida.Draw;
            
        }
        
        
        public void clasificar(){
            Teams.Sort();
            short contador = (short)Teams.Count;
            foreach (SoccerTeam item in Teams){
                item.Clasificacion = contador;
                contador--;
                
            }
        }

        public void onUpdateGame(SoccerTeam local,SoccerTeam visitante,String Score){ 
            
            List<Game> lista = GetGames(local.Equipo,visitante.Equipo,"07/07/2020 05:01:01 p. m.");
            if(!Games.Exists(c=>c.Local.Equipo== local.Equipo && c.Visitant.Equipo == visitante.Equipo&&c.fecha==Convert.ToDateTime("07/07/2020 05:01:01 p. m."))){
                Game nuevoJuego = new Game();
                nuevoJuego.Local = local;
                nuevoJuego.Visitant = visitante;
                nuevoJuego.fecha = Convert.ToDateTime("07/07/2020 05:01:01 p. m.");
                nuevoJuego.FullTimeResult = determinarPartido(Score,nuevoJuego);
               
                Games.Add(nuevoJuego);
                
                
            }else{             
                lista.ElementAt(0).FullTimeResult = determinarPartido(Score,lista.ElementAt(0));
            }
            Teams = llenarClasificacion(Games);            
            resultados();
        }
        public void resultados()
        {
           
           TableResults.mostrarResultados(Teams);
        }
        public static List<SoccerTeam> getTeams(string sectionNameLocal, string sectionNameVisitant){

    
            List<SoccerTeam> teamsOfGame = new List<SoccerTeam>();
            string [] formatLocalName = sectionNameLocal.Split('(');
            string nameOfLocalTeam = formatLocalName[0].TrimEnd();
            SoccerTeam teamLocal = new SoccerTeam(nameOfLocalTeam,0);
            string [] formatVisitantName = sectionNameVisitant.Split('(');
            string nameOfVisitantTeam = formatVisitantName[0].TrimEnd();
            SoccerTeam teamVisitant = new SoccerTeam(nameOfVisitantTeam,0);
            teamsOfGame.Add(teamLocal);
            teamsOfGame.Add(teamVisitant);
            return teamsOfGame;
        }
        

        public Game parseGame(string linea){
        
            string [] sections = linea.Split(',');
            if(sections.Length==6){
            List<SoccerTeam> teamsOfGame = getTeams(sections[2],sections[5]);
            DateTime date = formatTheDate(sections[1]);
            Game match = new Game();
            match.Local = teamsOfGame.ElementAt(0);
            match.Visitant = teamsOfGame.ElementAt(1);
            match.fecha = date;
            match.FullTimeResult = determinarPartido(sections[3],match);
            return match;
            }else{
                return null;
            }
        }
        public List<Game> saveGames(string [] listOfLines){
            Games = new List<Game>();
            foreach (string linea in listOfLines.Skip(1))
            {               
                Game game = parseGame(linea);
                Games.Add(game);
                
            }
            return Games;           
        }
        public static DateTime formatTheDate(string sectionDate){
                string generalPattern= "\\s*\\([A-z0-9]+\\)\\s*";
                Regex  result = new Regex(generalPattern);
                string[]formatDate = result.Split(sectionDate);               
                return Convert.ToDateTime(formatDate[1]);
        }
        
    }
    
    
}