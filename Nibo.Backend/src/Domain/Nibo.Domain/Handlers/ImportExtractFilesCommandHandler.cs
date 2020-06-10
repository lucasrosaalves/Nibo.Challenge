using FluentValidation.Results;
using MediatR;
using Nibo.Domain.Commands;
using Nibo.Domain.Entities;
using Nibo.Util.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nibo.Domain.Handlers
{
    public class ImportExtractFilesCommandHandler : CommandHandler, IRequestHandler<ImportExtractFilesCommand, ValidationResult>
    {
        public Task<ValidationResult> Handle(ImportExtractFilesCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return Task.FromResult(request.ValidationResult);
            }

            var extracts = new List<Extract>();

            extracts.AddRange(request.Files.Select(p => new Extract(p)));

            var accountsDetails = new List<AccountDetails>();
            foreach(var extract in extracts)
            {
                if(accountsDetails.Any(p=> p.Equals(extract.AccountDetails))) { continue; }

                accountsDetails.Add(extract.AccountDetails);
            }

            foreach (var accountDetail in accountsDetails)
            {
                // go to repositorty to get Account details

                var account = new Account(accountDetail.Bank, accountDetail.AccountNumber);

                var transactions = extracts.Where(p => p.AccountDetails.Equals(accountDetail)).SelectMany(p => p.Transactions);

                if (transactions.IsNullOrEmpty()) { continue; }

                account.AddTransactions(transactions);


                //save

            }


            


            return Task.FromResult(ValidationResult);
        }
    }
}
