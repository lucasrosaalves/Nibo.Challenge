using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nibo.Domain.Entities
{
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
}
