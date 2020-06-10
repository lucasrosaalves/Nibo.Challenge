using Nibo.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nibo.Domain.Entities
{
    public class Extract
    {
        private List<Transaction> _transactions;

        public string Bank { get; private set; }
        public string Account { get; private set; }
        public DateTime StartDate => _transactions.Min(p => p.Date);
        public DateTime EndDate => _transactions.Max(p => p.Date);
        public IReadOnlyCollection<Transaction> Transactions => _transactions ?? new List<Transaction>();


        protected Extract() { }

        public Extract(IEnumerable<string> lines)
        {
            if (lines is null) { return; }

            SetBank(lines);
            SetAccount(lines);
            SetTransacoes(lines);
        }

        private void SetBank(IEnumerable<string> lines)
        {
            Bank = GetFirstElementValueByTag(lines, ETag.BANKID);
        }

        private void SetAccount(IEnumerable<string> lines)
        {
            Account = GetFirstElementValueByTag(lines, ETag.ACCTID);
        }


        private void SetTransacoes(IEnumerable<string> lines)
        {
            _transactions = new List<Transaction>();
            var ranges = GetRangeOfElementsByTag(lines, ETag.STMTTRN);

            if (ranges is null || !ranges.Any()) { return; }

            foreach (var range in ranges)
            {
                var elements = range.GetElementsBetweenRange(lines);

                var type = GetFirstElementValueByTag(elements, ETag.TRNTYPE);
                var date = GetFirstElementDateByTag(elements, ETag.DTPOSTED);
                var value = GetFirstElementDecimalByTag(elements, ETag.TRNAMT);
                var description = GetFirstElementValueByTag(elements, ETag.MEMO);

                if (!value.HasValue)
                {
                    continue;
                }

                _transactions.Add(new Transaction(type, date, value.Value, description));
            }
        }

        private List<Position> GetRangeOfElementsByTag(IEnumerable<string> lines, ETag @tag)
        {
            var ranges = new List<Position>();
            string startTagNode = GetStartTagNode(@tag);
            string endTagNode = GetEndTagNode(@tag);

            for (int i = 0; i < lines.Count(); i++)
            {
                if (!lines.ElementAt(i).StartsWith(startTagNode)) { continue; }

                int start = i;
                int? end = GetClosestNodeTagPosition(lines, i, endTagNode);

                if (!end.HasValue) { continue; }

                ranges.Add(new Position(start, end.Value));
            }

            return ranges;
        }

        private int? GetClosestNodeTagPosition(IEnumerable<string> lines, int position, string nodeTag)
        {
            if (position + 1 >= lines.Count()) { return null; };

            for (int i = position + 1; i < lines.Count(); i++)
            {
                if (lines.ElementAt(i).StartsWith(nodeTag))
                {
                    return i;
                }
            }

            return null;
        }

        private string GetStartTagNode(ETag tag)
        {
            return string.Concat("<", @tag.ToString(), ">");
        }

        private string GetEndTagNode(ETag tag)
        {
            return string.Concat("</", @tag.ToString(), ">");
        }

        private string GetFirstElementValueByTag(IEnumerable<string> lines, ETag @tag)
        {
            string tagValue = GetStartTagNode(@tag);

            string element = lines.FirstOrDefault(p => p.StartsWith(tagValue));

            return element.Split('>').ElementAt(1);
        }

        private decimal? GetFirstElementDecimalByTag(IEnumerable<string> lines, ETag @tag)
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

        private DateTime GetFirstElementDateByTag(IEnumerable<string> lines, ETag @tag)
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

            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }
            return new DateTime(year, month, day, hour, minute, second);

        }
    }
}
