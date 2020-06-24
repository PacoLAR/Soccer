using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibreriaSoccer{
    public class Season{
        public List<SoccerTeam> Teams = new List<SoccerTeam>();
        public List<Game> Games = new List<Game>();
        public  List<SoccerTeam> ReadSeasonFromFile(string rutaarchivo){
            string patron = ("\\.csv$");
            Regex  nuevo = new Regex(patron);
            MatchCollection encontro = nuevo.Matches(rutaarchivo);                      
            if(encontro.Count>0){            
                try{
                    string [] lineas = File.ReadAllLines(rutaarchivo);            
                    foreach (string linea in lineas.Skip(1)){
                        string [] columnas = linea.Split(',');
                        string [] obtenernombrelocal = columnas[2].Split('(');
                        string nombreequipolocal = obtenernombrelocal[0].TrimEnd();
                        SoccerTeam equipolocal = new SoccerTeam(nombreequipolocal,0,0);
                        string [] obtenernombrevisitante = columnas[5].Split('(');
                        string nombreequipovisitante = obtenernombrevisitante[0].TrimEnd();
                        SoccerTeam equipovisitante = new SoccerTeam(nombreequipovisitante,0,0);
                        string patrondebusqueda= "\\s*\\([A-z0-9]+\\)\\s*";
                        Regex patron_dos = new Regex(patrondebusqueda);
                        string[]fecha_formateada = patron_dos.Split(columnas[1]);
                        Game partido = new Game();
                        partido.Local = equipolocal;
                        partido.Visitant = equipovisitante;
                        partido.fecha = Convert.ToDateTime(fecha_formateada[1]);
                        Games.Add(partido);
                        determinarPartido(columnas[3],partido);
                       
                    }
                   
                    var nombres = (from game in Games  select  (game.Local.Equipo)).Distinct().ToList();
                    foreach (var nombre in nombres)
                    {
                        llenarClasificacion(nombre);
                    }

                    clasificar();
                    Teams.Reverse();
                    
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

        public void llenarClasificacion(string nombre){
            List<Game> partidosjugados = (from game in Games where (game.Visitant.Equipo == nombre || game.Local.Equipo == nombre) select game).ToList();                   
            var tiposresultados = partidosjugados.GroupBy(partido =>{
                if(partido.Local.Equipo==nombre&&partido.FullTimeResult == ResultadosPartida.LocalWon){
                    return "ganado";
                }else if(partido.Visitant.Equipo==nombre&&partido.FullTimeResult == ResultadosPartida.VisitantWon){
                    return "ganado";
                }else{
                    return "empatado";
                }
            });

            SoccerTeam equipo = new SoccerTeam(nombre,0,0);
            foreach (var resultadopartido in tiposresultados){
                short partidos = Convert.ToInt16(resultadopartido.Count());
                if(resultadopartido.Key == "ganado"){
                    equipo.Puntos += Convert.ToInt16(partidos*3);
                }else{
                    equipo.Puntos += partidos;
                }
            }
            Teams.Add(equipo);                   
        }
        public void determinarPartido(string resultado,Game partido){
            string [] golesAnotados = resultado.Split('-');
            short golesEquipoLocal = Convert.ToInt16(golesAnotados[0]);
            short golesEquipoVisitante = Convert.ToInt16(golesAnotados[1]);
            
            if(golesEquipoLocal>golesEquipoVisitante){
             
                partido.FullTimeResult = ResultadosPartida.LocalWon;

            }else if(golesEquipoLocal<golesEquipoVisitante){
               
                partido.FullTimeResult = ResultadosPartida.VisitantWon;
            }else{
               
                partido.FullTimeResult = ResultadosPartida.Draw;
            }
        }
        public Game GetGame(string VisitantTeam = "", string Localteam ="", DateTime Date= default(DateTime)){
            return null;
        }
        public void clasificar(){
            Teams.Sort();
            short contador = (short)Teams.Count;
            foreach (SoccerTeam item in Teams){
                item.Clasificacion = contador;
                contador--;
            }
        }
    }
    
}