using System;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class Scraping : IEquatable<Scraping>
    {
        public string Schedule { get; set; }
        
        public bool Equals(Scraping other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Schedule?.Trim() == other.Schedule?.Trim();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Scraping);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode - this is providing the means to compare Scraping instances without having consumers become dependent on class members
            return Schedule?.Trim().GetHashCode() ?? 0;
        }
    }
}
