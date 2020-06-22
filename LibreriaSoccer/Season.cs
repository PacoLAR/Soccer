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
                        Game partido = new Game();
                        partido.Local = equipolocal;
                        partido.Visitant = equipovisitante;
                        Games.Add(partido);
                        if(!Teams.Exists(equipo => equipo.Equipo == equipolocal.Equipo)){
                            agregaPuntosAEquipo(equipolocal,equipovisitante,columnas[3]);
                            Teams.Add(equipolocal);
                        }
                        else if(!Teams.Exists(equipo => equipo.Equipo == equipovisitante.Equipo)){
                            Teams.Add(equipovisitante);
                        }
                        else{
                            SoccerTeam encontrarequipolocal = Teams.Find(equipo => equipo.Equipo.Contains(nombreequipolocal));
                            SoccerTeam encontrarequipovisitante = Teams.Find(equipo => equipo.Equipo.Contains(nombreequipovisitante));
                            agregaPuntosAEquipo(encontrarequipolocal,encontrarequipovisitante,columnas[3]);
                        }
                    }
                }catch (DirectoryNotFoundException ){
                    Console.WriteLine("No encontré el directorio");
                }catch (FileNotFoundException ){
                    Console.WriteLine("No encontré el archivo");
                }
                catch (IOException ){
                    Console.WriteLine("Error al leer el archivo");
                }
                clasificar();
                Teams.Reverse();
                return Teams;
            }else{
                Console.WriteLine("Solo archivos con formato csv");
                return null;
            }
            
        }
        public void agregaPuntosAEquipo(SoccerTeam local, SoccerTeam visitante, string resultado){
            string [] golesAnotados = resultado.Split('-');
            short golesEquipoLocal = Convert.ToInt16(golesAnotados[0]);
            short golesEquipoVisitante = Convert.ToInt16(golesAnotados[1]);
            local.GoalsScored +=golesEquipoLocal;
            local.GoalsRecived +=golesEquipoVisitante;
            visitante.GoalsScored += golesEquipoVisitante;
            visitante.GoalsRecived += golesEquipoLocal;           
            if(golesEquipoLocal>golesEquipoVisitante){
                local.Puntos +=3;
            }
            else if(golesEquipoLocal<golesEquipoVisitante){
                visitante.Puntos +=3;
            }else{
                visitante.Puntos+=1;
                local.Puntos+=1;
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