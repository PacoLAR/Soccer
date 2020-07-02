using System;
using Xunit;
using LibreriaSoccer;

namespace LibreriaSoccerTest
{
    public class SoccerTeamTest
    {
        
         [Fact ]
        public void ParseGameTest()
        {
            string input = "?,(Sat) 17 Aug 2019 (W33),RC Celta Vigo (1),1-3,0-1,Real Madrid (1)";
            Season temporada = new Season(null,string.Empty);
            var output = temporada.parseGame(input);
            Assert.Equal(output.Local.Equipo,"RC Celta Vigo");
            Assert.Equal(output.Visitant.Equipo,"Real Madrid");
            
        }
        [Theory]
        [InlineData("?,(Sat) 17 Aug 2019 (W33),RC Celta Vigo (1),1-3,0-1,Real Madrid (1)")]
        [InlineData("?,(Sun) 25 Aug 2019 (W34),Deportivo Alavés (2),0-0,0-0,RCD Espanyol (2)")]
        public void ParseGameTest(string line)
        {           
            Season temporada = new Season(null,string.Empty);
            Game output = temporada.parseGame(line);
            Assert.IsType(typeof(Game),output);
            Assert.StartsWith("?",line);            
        }

        [Theory]
        [InlineData("Soy,quien,quiero,ser")]
        public void ParseWrongGameTest(string line){
            Season temporada = new Season(null,string.Empty);
            Game output = temporada.parseGame(line);
            Console.WriteLine(output);
            Assert.Equal(output,null);
        }
    }
}
