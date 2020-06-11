using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nibo.API.Extensions;
using Nibo.API.ViewModels;
using Nibo.Domain.Commands;
using Nibo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nibo.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : MainController
    {
        private readonly IMediator _mediator;
        private readonly IAccountRepository _accountRepository;

        public AccountController(IMediator mediator,
            IAccountRepository accountRepository)
        {
            _mediator = mediator;
            _accountRepository = accountRepository;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Post([FromForm] FilesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var files = model.Files.Select(p => p.GetLines());

                var result = await _mediator.Send(new ImportExtractFilesCommand(files));

                return CustomResponse(result);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var accounts = await _accountRepository.GetAll();

                var result = accounts?.Select(a =>
                {
                    var transactions = 
                    a.Transactions
                    .OrderByDescending(p=> p.Date)
                    .Select(t => new TransactionViewModel(t.Date, t.Value, t.Description));

                    return new AccountViewModel(a.Details.Bank, a.Details.AccountNumber, a.CalculateBalance(), transactions);
                }) ?? new List<AccountViewModel>();

                return CustomResponse(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }

    }
}
