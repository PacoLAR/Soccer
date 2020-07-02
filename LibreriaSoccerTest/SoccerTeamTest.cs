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
    }
}
