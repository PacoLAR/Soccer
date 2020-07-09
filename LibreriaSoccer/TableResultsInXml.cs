using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LibreriaSoccer{
    public class TableResultsInXml : ITableResults
    {
        public void mostrarResultados(List<SoccerTeam> equipos)
        {
            
            
                using (TextWriter writer = new StreamWriter("resultxml.xml"))
                {
                    
                     XmlSerializer x = new XmlSerializer(equipos.GetType());
                     x.Serialize(writer,equipos);   
                    

                    writer.Close();
                }
                
                
        }
    }
}