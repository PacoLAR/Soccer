using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibreriaSoccer{
    public class CrearArchivoConTemporada : ITableResults
    {
        public void mostrarResultados(List<SoccerTeam> equipos){
            List<string> valoresDeTabla = new List<string>();
            string valores = "Equipo,Puntos,Clasificacion";
            valoresDeTabla.Add(valores);

            foreach (SoccerTeam equipo in equipos){
                
                string linea_de_equipo = ($"{equipo.Equipo},{equipo.Puntos},{equipo.Clasificacion}");
                valoresDeTabla.Add(linea_de_equipo);                
            }

            File.WriteAllLinesAsync("results.csv",valoresDeTabla);
            
        }
    }

}