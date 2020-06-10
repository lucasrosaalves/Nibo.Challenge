using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nibo.Domain.Entities;
using Nibo.Util.Extensions;

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
        public async Task<IActionResult> Post([FromForm] ImportExtractFilesCommand model)
        {
            try
            {
                if (model is null || model.Files.IsNullOrEmpty())
                {
                    return BadRequest();
                }

                var arquivos = new List<Extract>();

                foreach (var file in model.Files)
                {
                    if (file is null || file.Length == 0)
                    {
                        continue;
                    }

                    var linhas = new List<string>();

                    using (StreamReader sr = new StreamReader(file.OpenReadStream()))
                    {
                        string linha = string.Empty;

                        while ((linha = sr.ReadLine()) != null)
                        {
                            linha = linha.Trim();
                            linhas.Add(linha.Trim());
                        }
                    }

                    arquivos.Add(new Extract(linhas));

                }

                return Ok(arquivos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        public class ImportExtractFilesCommand
        {
            public IFormFile[] Files { get; set; }
        }

    }
}
