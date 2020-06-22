using System;

namespace LibreriaSoccer{
    public class SoccerTeam : IComparable<SoccerTeam>{

        public string Equipo{get;set;}
        public short Clasificacion {get;set;}
        public short Puntos{get;set;}
        public int GoalsScored{get;set;}
        public int GoalsRecived{get;set;}
            
        public SoccerTeam(string Equipo, short Clasificacion, short Puntos){
            this.Equipo = Equipo;
            this.Clasificacion = Clasificacion;
            this. Puntos = Puntos;
        }

        

        public override string ToString(){
            return $"equipo: {Equipo} puntos: {Puntos} clasificacion: {Clasificacion} goles anotados: {GoalsScored} goles recibidos: {GoalsRecived}";
        }

        public int CompareTo(SoccerTeam other)
        {
            return Puntos.CompareTo(other.Puntos);
        }
    }

    
}