﻿using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace Promitor.Core.Configuration.Model.AzureMonitor
{
    public class AzureMonitorLoggingConfiguration
    {
        public HttpLoggingDelegatingHandler.Level InformationLevel { get; set; } = HttpLoggingDelegatingHandler.Level.Basic;
        public bool IsEnabled { get; set; } = false;
    }
}