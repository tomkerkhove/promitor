using System.ComponentModel;
using Bogus;

namespace Promitor.Tests.Unit
{
    [Category("Unit")]
    public class UnitTest
    {
        protected Faker BogusGenerator { get; } = new();
    }
}