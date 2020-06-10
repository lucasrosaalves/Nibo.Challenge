using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nibo.API.Extensions;
using Nibo.API.ViewModels;
using Nibo.Domain.Commands;
using Nibo.Domain.Entities;
using Nibo.Util.Extensions;

namespace Nibo.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : MainController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Post([FromForm] FilesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var files = model.Files.Select(p => p.GetLines());

                var resultado = await _mediator.Send(new ImportExtractFilesCommand(files));

                return CustomResponse(resultado);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }

        }

    }
}
