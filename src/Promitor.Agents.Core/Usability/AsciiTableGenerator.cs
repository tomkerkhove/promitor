using Spectre.Console;

namespace Promitor.Agents.Core.Usability
{
    public class AsciiTableGenerator
    {
        protected Table CreateAsciiTable(string caption = null)
        {
            var asciiTable = new Table
            {
                Border = TableBorder.Rounded
            };

            if(string.IsNullOrWhiteSpace(caption) == false)
            {
                asciiTable.Caption(caption);
            }

            return asciiTable;
        }
    }
}