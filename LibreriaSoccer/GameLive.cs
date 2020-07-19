using System;

namespace LibreriaSoccer{
    public class GameLive:Game{

        private Boolean PartidoJugandose{get;set;}
        public String Score {get;set;}
        public String  Fecha{get;set;}
        public GameLive(SoccerTeam local, SoccerTeam visitante){
            this.Local = local;
            this.Visitant = visitante;
            PartidoJugandose = true;
            Score = "0-0";
            
        }
        public delegate void updateGameHandler(SoccerTeam local, SoccerTeam visitante,string resultado);
        public event updateGameHandler updateGame;

        public void onUpdateGame(SoccerTeam local,SoccerTeam visitante){
            if(updateGame!=null){
                updateGame(Local,Visitant,Score);
                
            }           
        }
        public void VisitantScore(){
            if(PartidoJugandose){                        
                int puntaje = Convert.ToInt32(Score.Substring(2));
                puntaje+=1;
                Score = $"{Score.Substring(0,2)}{puntaje}";            
                onUpdateGame(Local,Visitant);
            }else{
                Console.WriteLine("El partido termino no es posible anotar mas goles");
            }
        }
        public void LocalScore(){
            if(PartidoJugandose){            
                int puntaje = Convert.ToInt32(Score.Substring(0,1));
                puntaje+=1;
                Score = $"{puntaje}{Score.Substring(1)}";            
            onUpdateGame(Local,Visitant);
            }else{
                Console.WriteLine("El partido termino no es posible anotar mas goles");
            }
        }
        public void finishGame(){
            PartidoJugandose = false;
            Console.WriteLine("Partido terminado");
        }
    }
}