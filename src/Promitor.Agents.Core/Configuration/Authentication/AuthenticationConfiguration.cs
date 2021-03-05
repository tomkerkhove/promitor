using System;
using System.Collections.Generic;
using System.Text;

namespace Promitor.Agents.Core.Configuration.Authentication
{
    public class AuthenticationConfiguration
    {
        public AuthenticationMode Mode { get; set; } = Defaults.Authentication.Mode;

    }
}
