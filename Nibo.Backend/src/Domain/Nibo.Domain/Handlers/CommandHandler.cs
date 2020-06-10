using FluentValidation.Results;
using Nibo.Domain.Repositories;
using System.Threading.Tasks;

namespace Nibo.Domain.Handlers
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> CommitData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddErro("Transaction erro");

            return ValidationResult;
        }
    }
}
