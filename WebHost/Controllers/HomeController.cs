using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsertNamespace.Models;
using InsertNamespace.Logging;
using IdentityServer4.Services;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Authorization;

namespace InsertNamespace.Controllers
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private ILog _log;
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(ILog log, IIdentityServerInteractionService interaction)
        {
            _log = log;
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}
