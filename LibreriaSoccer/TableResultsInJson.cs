
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace LibreriaSoccer{
    public class TableResultsInJson : ITableResults
    {
        public void mostrarResultados(List<SoccerTeam> equipos)
        {
            
            
                using (TextWriter writer = new StreamWriter("resultjson.json"))
                {

                    string json = JsonSerializer.Serialize(equipos);
                    writer.Write(json);
                    writer.Close();
                }
                
        }
    }
}