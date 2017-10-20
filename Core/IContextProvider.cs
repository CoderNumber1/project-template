using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace
{
    public interface IContextProvider
    {
        // Server Information
        string GetMachineName();

        string GetMachineOS();

        string GetMachineIpAddress();

        // User Information
        string GetSubjectId();

        // User Agent Information
        string GetUserIpAddress();

        string GetUserAgent();

    }
}
