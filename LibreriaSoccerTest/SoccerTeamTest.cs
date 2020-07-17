using System;
using Xunit;
using LibreriaSoccer;

namespace LibreriaSoccerTest
{
    public class SoccerTeamTest
    {
        
         
        [Theory]
        [InlineData("?,(Sat) 17 Aug 2019 (W33),RC Celta Vigo (1),1-3,0-1,Real Madrid (1)")]
        [InlineData("?,(Sun) 25 Aug 2019 (W34),Deportivo Alavés (2),0-0,0-0,RCD Espanyol (2)")]
        //Buscar ganador como visitante
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
        
        [Theory]
        [InlineData("1-0")]
        [InlineData("5-1")]
        public void determinarPartidoGanadoLocalTest(String resultado){
            Season temporada = new Season(null,String.Empty);
            ResultadosPartida resultadofinal =temporada.determinarPartido(resultado,null);
            Assert.IsType(typeof(ResultadosPartida),resultadofinal);
            Assert.Equal(resultadofinal,ResultadosPartida.LocalWon);
        }
        [Theory]
        [InlineData("0-2")]
        public void determinarPartidoGanadoVisitanteTest(String resultado){
            Season temporada = new Season(null,String.Empty);
            ResultadosPartida resultadofinal =temporada.determinarPartido(resultado,null);
            Assert.IsType(typeof(ResultadosPartida),resultadofinal);
            Assert.Equal(resultadofinal,ResultadosPartida.VisitantWon);
        }
        [Theory]
        [InlineData("0-0")]
        public void determinarPartidoEmpatadoTest(String resultado){
            Season temporada = new Season(null,String.Empty);            
            ResultadosPartida resultadofinal =temporada.determinarPartido(resultado,null);
            Assert.IsType(typeof(ResultadosPartida),resultadofinal);
            Assert.Equal(resultadofinal,ResultadosPartida.Draw);
        }
        

    }
}
