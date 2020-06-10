using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> Post([FromForm] MyFileUploadClass model)
        {
            try
            {
                if(model is null || model.Files.IsNullOrEmpty())
                {
                    return BadRequest();
                }

                var arquivos = new List<Arquivo>();

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

                    arquivos.Add(new Arquivo(linhas));

                }

                return Ok(arquivos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        public class MyFileUploadClass
        {
            public IFormFile[] Files { get; set; }
        }

        public class Arquivo
        {
            private List<Transacao> _transacoes;

            public string Banco { get; private set; }
            public string Conta { get; private set; }
            public DateTime DataInicio { get; private set; }
            public DateTime DataFim { get; private set; }
            public IReadOnlyCollection<Transacao> Transacoes => _transacoes ?? new List<Transacao>();

            protected Arquivo() { }

            public Arquivo(IEnumerable<string> lines)
            {
                if(lines is null) { return; }

                SetBank(lines);
                SetAccount(lines);
                SetStartDate(lines);
                SetEndDate(lines);
                SetTransacoes(lines);
            }

            private void SetBank(IEnumerable<string> lines)
            {
                Banco = GetFirstElementValueByTag(lines, Tag.BANKID);
            }

            private void SetAccount(IEnumerable<string> lines)
            {
                Conta = GetFirstElementValueByTag(lines, Tag.ACCTID);
            }

            private void SetStartDate(IEnumerable<string> lines)
            {
                DataInicio = GetFirstElementDateByTag(lines, Tag.DTSTART);
            }

            private void SetEndDate(IEnumerable<string> lines)
            {
                DataFim = GetFirstElementDateByTag(lines, Tag.DTEND);
            }

            private void SetTransacoes(IEnumerable<string> lines)
            {
                _transacoes = new List<Transacao>();
                var ranges = GetRangeOfElementsByTag(lines, Tag.STMTTRN);

                if(ranges is null || !ranges.Any()) { return; }

                foreach(var range in ranges)
                {
                    var elements = range.GetElementsBetweenRange(lines);

                    var type = GetFirstElementValueByTag(elements, Tag.TRNTYPE);
                    var date = GetFirstElementDateByTag(elements, Tag.DTPOSTED);
                    var value = GetFirstElementDecimalByTag(elements, Tag.TRNAMT);
                    var description = GetFirstElementValueByTag(elements, Tag.MEMO);


                    _transacoes.Add(new Transacao(type, date, value, description));

                }
            }

            private List<Position> GetRangeOfElementsByTag(IEnumerable<string> lines, Tag @tag)
            {
                var ranges = new List<Position>();
                string startTagNode = GetStartTagNode(@tag);
                string endTagNode = GetEndTagNode(@tag);

                for (int i = 0; i < lines.Count(); i++)
                {
                    if (!lines.ElementAt(i).StartsWith(startTagNode)) { continue; }

                    int start = i;
                    int? end = GetClosestNodeTagPosition(lines, i, endTagNode);

                    if(!end.HasValue) { continue; }

                    ranges.Add(new Position(start, end.Value));
                }

                return ranges;
            }

            private int? GetClosestNodeTagPosition(IEnumerable<string> lines, int position, string nodeTag)
            {
                if(position + 1 >= lines.Count()) { return null; };

                for (int i = position + 1; i < lines.Count(); i++)
                {
                    if (lines.ElementAt(i).StartsWith(nodeTag))
                    {
                        return i;
                    }
                }

                return null;
            }

            private string GetStartTagNode(Tag tag)
            {
                return string.Concat("<", @tag.ToString(), ">");
            }

            private string GetEndTagNode(Tag tag)
            {
                return string.Concat("</", @tag.ToString(), ">");
            }

            private string GetFirstElementValueByTag(IEnumerable<string> lines, Tag @tag)
            {
                string tagValue = GetStartTagNode(@tag);

                string element = lines.FirstOrDefault(p => p.StartsWith(tagValue));

                return element.Split('>').ElementAt(1);
            }

            private decimal? GetFirstElementDecimalByTag(IEnumerable<string> lines, Tag @tag)
            {
                string value = GetFirstElementValueByTag(lines, @tag);

                try
                {
                    return Convert.ToDecimal(value);
                }
                catch
                {
                    return null;
                }
            }

            private DateTime GetFirstElementDateByTag(IEnumerable<string> lines, Tag @tag)
            {
                string value = GetFirstElementValueByTag(lines, @tag);

                if (string.IsNullOrWhiteSpace(value)) { return DateTime.MinValue; }

                var sDate = new string(value.Where(char.IsDigit).ToArray());

                if (string.IsNullOrWhiteSpace(sDate)) { return DateTime.MinValue; }

                int year = int.Parse(sDate.Substring(0, 4));
                int month = int.Parse(sDate.Substring(4, 2));
                int day = int.Parse(sDate.Substring(6, 2));
                int hour = sDate.Length >= 10 ? int.Parse(sDate.Substring(8, 2)) : 0;
                int minute = sDate.Length >= 12 ? int.Parse(sDate.Substring(10, 2)) : 0;
                int second = sDate.Length >= 14 ? int.Parse(sDate.Substring(12, 2)) : 0;

                return new DateTime(year, month, day, hour, minute, second);
            }
        }


        public class Position
        {
            public Position(int start, int end)
            {
                Start = start;
                End = end;
            }

            public int Start { get; private set; }
            public int End { get; private set; }

            public List<string> GetElementsBetweenRange(IEnumerable<string> elements)
            {
                var response = new List<string>();
                for (int i = Start + 1; i < End; i++)
                {
                    response.Add(elements.ElementAt(i));
                }

                return response;
            }         
        }

        public class Transacao
        {

            protected Transacao() { }

            public Transacao(
                string operacao, 
                DateTime dataOperacao,
                decimal? valor, 
                string descricao)
            {
                Operacao = operacao;
                DataOperacao = dataOperacao;
                Valor = valor;
                Descricao = descricao;
            }

            public string Operacao { get; private set; }
            public DateTime DataOperacao { get;  private set; }
            public decimal? Valor { get; private set; }
            public string Descricao { get; private set; }


        }

        public enum Tag
        {
            BANKID,
            ACCTID,
            DTSTART,
            DTEND,
            STMTTRN,
            TRNTYPE,
            DTPOSTED,
            TRNAMT,
            MEMO
        }
    }
}
