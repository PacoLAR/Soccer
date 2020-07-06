using System;

namespace LibreriaSoccer{
    public class GameLive{


        public delegate void updateGameHandler();
        public event updateGameHandler updateGame;

        public void onUpdateGame(){
            if(updateGame!=null){
                updateGame();
            }
           
            
        }
    }
}