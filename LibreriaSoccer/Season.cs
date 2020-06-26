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
            
        }

        ITableResults TableResults{get;set;}
        string FileLocation{get;set;}
        public List<SoccerTeam> Teams = new List<SoccerTeam>();
        public List<Game> Games = new List<Game>();
        public  List<SoccerTeam> ReadSeasonFromFile(){
            string patron = ("\\.csv$");
            Regex  nuevo = new Regex(patron);
            MatchCollection encontro = nuevo.Matches(FileLocation);                      
            if(encontro.Count>0){            
                try{
                    string [] lineas = File.ReadAllLines(FileLocation);            
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
            
            var puntos = Games.Sum(c=> (c.FullTimeResult == ResultadosPartida.LocalWon && c.Local.Equipo == nombre)?3
                                    :(c.FullTimeResult == ResultadosPartida.VisitantWon && c.Visitant.Equipo == nombre)?3
                                    :(c.FullTimeResult == ResultadosPartida.Draw && (c.Local.Equipo == nombre || c.Visitant.Equipo == nombre))?1:0);   

            short puntosobtenidos = Convert.ToInt16(puntos);
            SoccerTeam equipo = new SoccerTeam(nombre,0,puntosobtenidos);
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

        public void resultados(){
            TableResults.mostrarResultados(Teams);
        }
    }
    
}