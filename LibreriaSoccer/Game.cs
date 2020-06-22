using System;

namespace LibreriaSoccer{
    public class Game{
        public DateTime Date{get;set;}
        public SoccerTeam Visitant{get;set;}
        public SoccerTeam Local{get;set;}

        public ResultadosPartida HalfTimeResult{get;set;}
        public ResultadosPartida FullTimeResult{get;set;}
        
        
    }
}