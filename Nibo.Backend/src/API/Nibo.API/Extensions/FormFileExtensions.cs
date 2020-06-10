using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace Nibo.API.Extensions
{
    public static class FormFileExtensions
    {
        public static List<string> GetLines(this IFormFile @formFile)
        {
            var lines = new List<string>();

            using (StreamReader sr = new StreamReader(@formFile.OpenReadStream()))
            {
                string linha = string.Empty;

                while (!((linha = sr.ReadLine()) is null))
                {
                    linha = linha.Trim();
                    lines.Add(linha.Trim());
                }
            }

            return lines;
        }
    }
}
