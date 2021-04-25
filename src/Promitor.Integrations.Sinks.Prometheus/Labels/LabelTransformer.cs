using System;
using System.Collections.Generic;
using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Integrations.Sinks.Prometheus.Labels
{
    public static class LabelTransformer
    {
        public static Dictionary<string, string> TransformLabels(LabelTransformation transformation, Dictionary<string, string> labels)
        {
            switch (transformation)
            {
                case LabelTransformation.None:
                    return labels;
                case LabelTransformation.Lowercase:
                    return TransformToLowercase(labels);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Dictionary<string, string> TransformToLowercase(Dictionary<string, string> labels)
        {
            Dictionary<string, string> transformedLabels = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> label in labels)
            {
                transformedLabels.Add(label.Key, label.Value.ToLower());
            }

            return transformedLabels;
        }
    }
}
