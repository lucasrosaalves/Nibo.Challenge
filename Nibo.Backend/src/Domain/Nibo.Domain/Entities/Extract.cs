using Nibo.Domain.Enum;
using Nibo.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nibo.Domain.Entities
{
    public class Extract
    {
        private List<Transaction> _transactions;

        public AccountDetails AccountDetails { get; private set; }
        public DateTime StartDate => _transactions.Min(p => p.Date);
        public DateTime EndDate => _transactions.Max(p => p.Date);
        public IReadOnlyCollection<Transaction> Transactions => _transactions ?? new List<Transaction>();

        protected Extract() { }

        public Extract(IEnumerable<string> lines)
        {
            if (lines is null) { return; }

            SetAccountDetails(lines);
            SetTransactions(lines);
        }

        private void SetAccountDetails(IEnumerable<string> lines)
        {
            var bank = GetFirstElementByTag(lines, ETag.BANKID);
            var accountNumber = GetFirstElementByTag(lines, ETag.ACCTID);

            AccountDetails = new AccountDetails(bank, accountNumber);
        }

        private void SetTransactions(IEnumerable<string> lines)
        {
            _transactions = new List<Transaction>();
            var ranges = GetRangeOfElementsByTag(lines, ETag.STMTTRN);

            if (ranges is null || !ranges.Any()) { return; }

            foreach (var range in ranges)
            {
                var elements = range.GetElementsBetweenRange(lines);

                var type = GetFirstElementByTag(elements, ETag.TRNTYPE);
                var date = GetFirstDateElementByTag(elements, ETag.DTPOSTED);
                var value = GetFirstElementByTag(elements, ETag.TRNAMT).TryParseDecimal();
                var description = GetFirstElementByTag(elements, ETag.MEMO);

                if (!value.HasValue || !date.HasValue)
                {
                    continue;
                }

                _transactions.Add(new Transaction(type, date.Value, value.Value, description));
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

        private string GetFirstElementByTag(IEnumerable<string> lines, ETag @tag)
        {
            string tagValue = GetStartTagNode(@tag);

            string element = lines.FirstOrDefault(p => p.StartsWith(tagValue));

            if (element.IsNullOrWhiteSpace()) { return string.Empty; }

            return element.Split('>')?.ElementAt(1);
        }

        private DateTime? GetFirstDateElementByTag(IEnumerable<string> lines, ETag @tag)
        {
            string value = GetFirstElementByTag(lines, @tag);

            var sDate = value.KeepOnlyNumericValue();

            if (string.IsNullOrWhiteSpace(sDate)) { return null; }

            int? year = sDate.TryParseInt(0, 4);
            int? month = sDate.TryParseInt(4, 2);
            int? day = sDate.TryParseInt(6, 2);

            if (!year.HasValue || !month.HasValue || !day.HasValue)
            {
                return null;
            }

            int daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }

            int hour = sDate.TryParseInt(8, 2) ?? 0;
            int minute = sDate.TryParseInt(10, 2) ?? 0;
            int second = sDate.TryParseInt(12, 2) ?? 0;

            return new DateTime(
                year.Value,
                month.Value,
                day.Value,
                hour,
                minute,
                second)
                .ToUniversalTime();
        }
    }
}
