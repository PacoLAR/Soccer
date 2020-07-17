using System;

namespace LibreriaSoccer{
    public class Game{
        public DateTime fecha{get;set;}
        public SoccerTeam Visitant{get;set;}
        public SoccerTeam Local{get;set;}
        public int GoalsLocal{get;set;}
        public int GoalsVisitant{get;set;}

        public ResultadosPartida HalfTimeResult{get;set;}
        public ResultadosPartida FullTimeResult{get;set;}
        
        
        public override string ToString(){
            return $"Equipo local: {Local.Equipo} Equipo Visitante: {Visitant.Equipo} Resultado: {FullTimeResult} Fecha: {fecha} Goles Locales: {GoalsLocal} Goles visitante: {GoalsVisitant}";

        }

    }

    
}