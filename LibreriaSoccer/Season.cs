using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibreriaSoccer{
    public class Season{  
        public Season(ITableResults TableResults,string FileLocation){
            this.TableResults = TableResults;
            this.FileLocation = FileLocation;
            Teams = new List<SoccerTeam>();
            Games = new List<Game>();
            GameLive juego = new GameLive();
            juego.updateGame += onUpdateGame;
            
        }

        ITableResults TableResults{get;set;}
        string FileLocation{get;set;}
        public List<SoccerTeam> Teams{get;private set;}
        public List<Game> Games {get;private set;}

       
        public  List<SoccerTeam> ReadSeasonFromFile(){
            string patron = ("\\.csv$");
            Regex  nuevo = new Regex(patron);
            MatchCollection encontro = nuevo.Matches(FileLocation);                      
            if(encontro.Count>0){            
                try{
                    string [] lineas = File.ReadAllLines(FileLocation);
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
            var puntos = Games.Sum(c=> (c.FullTimeResult == ResultadosPartida.LocalWon && c.Local.Equipo == nombre)?3
                                :(c.FullTimeResult == ResultadosPartida.VisitantWon && c.Visitant.Equipo == nombre)?3
                                :(c.FullTimeResult == ResultadosPartida.Draw && (c.Local.Equipo == nombre || c.Visitant.Equipo == nombre))?1:0);   

            short puntosobtenidos = Convert.ToInt16(puntos);
            SoccerTeam equipo = new SoccerTeam(nombre,puntosobtenidos);
            Teams.Add(equipo);
            Console.WriteLine("*****************************************************");
            clasificar();
            Teams.Reverse();
            onUpdateGame();
            }
            return Teams;                              
        }
        public List<Game> GetGames(string localTeam =null,string visitantTeam=null,string date = null ){
            DateTime Date;         
            if((!string.IsNullOrEmpty(localTeam)&&!string.IsNullOrEmpty(visitantTeam)&&!string.IsNullOrEmpty(date))){
                Date = Convert.ToDateTime(date);
                return Games.FindAll(game => game.Local.Equipo==localTeam&&game.Visitant.Equipo==visitantTeam&&game.fecha==Date);
            }else if((!string.IsNullOrEmpty(localTeam)&&!string.IsNullOrEmpty(visitantTeam))){
                return Games.FindAll(game => game.Local.Equipo==localTeam&&game.Visitant.Equipo==visitantTeam);
            }else if(!string.IsNullOrEmpty(localTeam)&&!string.IsNullOrEmpty(date)){
                Date = Convert.ToDateTime(date);
                return Games.FindAll(game => game.Local.Equipo==localTeam&&game.fecha==Date);
            }else if(!string.IsNullOrEmpty(visitantTeam)&&!string.IsNullOrEmpty(date)){
                Date = Convert.ToDateTime(date);
                return Games.FindAll(game => game.Visitant.Equipo==visitantTeam&&game.fecha==Date);
            }else if(!string.IsNullOrEmpty(localTeam)){
                return Games.FindAll(Game=>Game.Local.Equipo == localTeam);
            }else if(!string.IsNullOrEmpty(visitantTeam)){
                return Games.FindAll(Game=>Game.Visitant.Equipo==visitantTeam);
            }else if(!string.IsNullOrEmpty(date)){
                Date = Convert.ToDateTime(date);
                return Games.FindAll(game => game.fecha==Date);                
            }else{
                return Games;
            }
            
        }
        
        public ResultadosPartida determinarPartido(string resultado){
            string [] golesAnotados = resultado.Split('-');
            short golesEquipoLocal = Convert.ToInt16(golesAnotados[0]);
            short golesEquipoVisitante = Convert.ToInt16(golesAnotados[1]);
            
            return  (golesEquipoLocal>golesEquipoVisitante) ?ResultadosPartida.LocalWon
            :(golesEquipoLocal<golesEquipoVisitante)? ResultadosPartida.LocalWon
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

        public void onUpdateGame(){           
            resultados();        
        }
        public void resultados(){
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
            match.FullTimeResult = determinarPartido(sections[3]);
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