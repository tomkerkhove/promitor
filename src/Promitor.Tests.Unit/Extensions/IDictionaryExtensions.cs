// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    internal static class IDictionaryExtensions
    {
        public static void ForAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<KeyValuePair<TKey, TValue>> action)
        {
            foreach (var entry in dictionary)
            {
                action(entry);
            }
        }
    }
}
