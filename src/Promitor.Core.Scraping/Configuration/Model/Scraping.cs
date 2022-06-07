using System;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class Scraping : IEquatable<Scraping>
    {
        public string Schedule { get; set; }

        
        #region Equality
        
        public bool Equals(Scraping other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Schedule?.Trim() == other.Schedule?.Trim();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Scraping);
        }

        public override int GetHashCode()
        {
            return Schedule?.Trim().GetHashCode() ?? 0;
        }
        
        #endregion
    }
}
