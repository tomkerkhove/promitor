using Newtonsoft.Json;

namespace Promitor.Agents.Scraper.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Clones an object without a reference, the ugly way.
        /// </summary>
        /// <typeparam name="TObject">Type of the object to be cloned</typeparam>
        /// <param name="initialObject">Initial object to clone</param>
        public static TObject Clone<TObject>(this TObject initialObject)
        {
            var rawObject = JsonConvert.SerializeObject(initialObject);
            return JsonConvert.DeserializeObject<TObject>(rawObject);
        }
    }
}