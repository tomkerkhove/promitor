﻿using Promitor.Agents.Core.Configuration.Authentication;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;

namespace Promitor.Agents.Core.Configuration
{
    public class RuntimeConfiguration
    {
        public ServerConfiguration Server { get; set; } = new ServerConfiguration();
        public AuthenticationConfiguration Authentication { get; set; } = new AuthenticationConfiguration();
        public TelemetryConfiguration Telemetry { get; set; } = new TelemetryConfiguration();
    }
}