using FluentValidation;
using System.Collections.Generic;

namespace Nibo.Domain.Commands
{
    public class ImportExtractFilesCommand : Command
    {
        public ImportExtractFilesCommand(IEnumerable<IEnumerable<string>> files)
        {
            Files = files;
        }

        public IEnumerable<IEnumerable<string>> Files { get; }

        public override bool IsValid()
        {
            ValidationResult = new ImportExtractFilesCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class ImportExtractFilesCommandValidation : AbstractValidator<ImportExtractFilesCommand>
        {
            public ImportExtractFilesCommandValidation()
            {
                RuleFor(c => c.Files)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Files can not be null or empty");
            }

        }
    }
}
