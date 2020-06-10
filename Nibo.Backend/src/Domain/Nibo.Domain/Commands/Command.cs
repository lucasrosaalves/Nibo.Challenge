using FluentValidation.Results;
using MediatR;
using System;

namespace Nibo.Domain.Commands
{
    public abstract class Command : IRequest<ValidationResult>
    {
        public ValidationResult ValidationResult { get; set; }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
