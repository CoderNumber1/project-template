using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InsertNamespace
{
    public class WebContextProvider : IContextProvider
    {
        private IHttpContextAccessor _contextAccessor;

        public WebContextProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetMachineIpAddress()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName())
                    ?.AddressList
                    ?.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    ?.ToString();
            }
            catch (Exception ex)
            {
                // TODO: Need an internal logger to write to.
            }

            return null;
        }

        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        public string GetMachineOS()
        {
            return Environment.OSVersion.ToString();
        }

        public string GetSubjectId()
        {
            return _contextAccessor?.HttpContext?.User?.Identity?.Name;
        }

        public string GetUserAgent()
        {
            try
            {
                return _contextAccessor?.HttpContext?.Request?.Headers["User-Agent"].ToString();
            }
            catch (Exception ex)
            {
                // TODO: Internal logging.
            }

            return null;
        }

        public string GetUserIpAddress()
        {
            return _contextAccessor?.HttpContext?.Request?.Host.Host;
        }
    }
}
