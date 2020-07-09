using System;
using System.Collections;
using System.Collections.Generic;

namespace LibreriaSoccer{
    public class TablaDeResultados : ITableResults
    {

        public void mostrarResultados(List<SoccerTeam> equipos)
        {
            
            foreach (SoccerTeam equipo in equipos){
                Console.WriteLine(equipo);
            }
            
        }
    }

}