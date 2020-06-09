using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nibo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MyFileUploadClass @class)
        {
            try
            {
                var files = @class.Files;
                long tamanhoArquivos = files.Sum(f => f.Length);
                var caminhoArquivo = Path.GetTempFileName();

                foreach (var file in files)
                {        
                    if (file is null || file.Length == 0)
                    {
                        continue;
                    }

                    StringBuilder resultado = new StringBuilder();
                    int nivel = 0;

                    var content = file.OpenReadStream();

                    using (StreamReader sr = new StreamReader(content))
                    {
                        //string linha = await sr.ReadToEndAsync();
                        string linha = string.Empty;

                        while ((linha = sr.ReadLine()) != null)
                        {
                            linha = linha.Trim();

                            if (linha.StartsWith("</") && linha.EndsWith(">"))
                            {
                                AddTabs(resultado, nivel, true);
                                nivel--;
                                resultado.Append(linha);
                            }
                            else if (linha.StartsWith("<") && linha.EndsWith(">"))
                            {
                                nivel++;
                                AddTabs(resultado, nivel, true);
                                resultado.Append(linha);
                            }
                            else if (linha.StartsWith("<") && !linha.EndsWith(">"))
                            {
                                AddTabs(resultado, nivel + 1, true);
                                resultado.Append(linha);
                                resultado.Append(ReturnFinalTag(linha));
                            }
                        }
                    }


                }

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        private static void AddTabs(StringBuilder stringObject, int lengthTabs, bool newLine)
        {
            if (newLine)
            {
                stringObject.AppendLine();
            }
            for (int j = 1; j < lengthTabs; j++)
            {
                stringObject.Append("\t");
            }
        }

        private static String ReturnFinalTag(String content)
        {
            String returnFinal = "";

            if ((content.IndexOf("<") != -1) && (content.IndexOf(">") != -1))
            {
                int position1 = content.IndexOf("<");
                int position2 = content.IndexOf(">");
                if ((position2 - position1) > 2)
                {
                    returnFinal = content.Substring(position1, (position2 - position1) + 1);
                    returnFinal = returnFinal.Replace("<", "</");
                }
            }

            return returnFinal;
        }

        public class MyFileUploadClass
        {
            public IFormFile[] Files { get; set; }
        }
    }
}
