namespace Promitor.Scraper.Host
{
    internal static class Constants
    {
        internal static class Texts
        {
            internal const string Welcome = @"██████╗ ██████╗  ██████╗ ███╗   ███╗██╗████████╗ ██████╗ ██████╗ 
██╔══██╗██╔══██╗██╔═══██╗████╗ ████║██║╚══██╔══╝██╔═══██╗██╔══██╗
██████╔╝██████╔╝██║   ██║██╔████╔██║██║   ██║   ██║   ██║██████╔╝
██╔═══╝ ██╔══██╗██║   ██║██║╚██╔╝██║██║   ██║   ██║   ██║██╔══██╗
██║     ██║  ██║╚██████╔╝██║ ╚═╝ ██║██║   ██║   ╚██████╔╝██║  ██║
╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚═╝     ╚═╝╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝";

            internal static class Validation
            {
                internal const string SeperationText = "-------------------------------------------------";
                internal const string StartValidationText = "Starting validation of configuration...";
                internal const string ValidationStepSuccessfulText = "\tValidation step succeeded.";
                internal const string GeneralValidationFailed =
                    "Configuration is invalid. Please fix them before running Promitor again";
                internal const string GeneralValidationSucceeded = "Configuration is valid.";
            }
        }
    }
}