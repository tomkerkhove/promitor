# Improving Validation

The goal of this change is to improve the validation in Promitor to make it easier
for users to find problems with their configuration, and to make it easier to add
validation into the deserializers.

## Must Haves

- A consistent output format that explains exactly what the problem is, and ideally
  where it is.
- We must indicate when unknown field names are specified so we can highlight typos.

## Nice to Haves

- A schema for the configuration files so we have a single source of truth for the
  configuration format.
- Contextual output showing snippets of the parts of the file that have issues.
- Suggestions for typos (e.g. 'tneantId' instead of 'tenantId').

## Output Format

To make it easy for users to fix problems with the configuration file, we need to
tell them the filename, line number and ideally the column where the error occurs.

To achieve this we will use the following format:

```text
<filename>:<line-number>:<column>: <message>
```

We may want to remove the filename from the error messages or make it optional since
we might be reporting multiple errors in the same file. In which case we could use
something like this:

```text
Errors were found in `/config/metric-config.yaml`:

1:1: 'azureMetadata' is a required field but was not found.
3:1: 'azureMatadata' is not a valid field name. Did you mean 'azureMetadata'?
```

## Contextual Output

We should be able to output annotated snippets of the file to make it really easy
to spot problems.

For example, if we have the following configuration:

```yaml
version: v1

azureMatadata:
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptionId: 0f9d7fea-99e8-4768-8672-06a28514f77e
  resourceGroupName: promitor
```

We should be able to output something like this:

```text
Errors were found in `/config/metric-config.yaml`:

1:1: 'azureMetadata' is a required field but was not found.
3:1: 'azureMatadata' is not a valid field name. Did you mean 'azureMetadata'?

version: v1

azureMatadata:
^^^^^^^^^^^^^

...
```

## Scenarios

### Unknown Configuration Field

```yaml
version: v1

azureMatadata:
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptionId: 0f9d7fea-99e8-4768-8672-06a28514f77e
  resourceGroupName: promitor
```

Expected result:

```text
3:1: Unknown field `azureMatadata`
```

Ideal result:

```text
3:1: Unknown field `azureMatadata`. Did you mean `azureMetadata`?
```

### Missing Required Property

```yaml
version: v1

azureMetadata:
  #tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptionId: 0f9d7fea-99e8-4768-8672-06a28514f77e
  resourceGroupName: promitor
```

Expected result:

```text
3:1: 'tenantId' is a required field but was not found.
```

### Invalid Format / Type

```yaml
version: v1

metricDefaults:
  aggregation:
    interval: true
```

Expected result:

```text
5:15: 'interval' must be in the format 'HH:mm:ss', but 'true' was provided.

metricDefaults:
  aggregation:
    interval: true
              ^^^^
```

### Duplicate Field Detection

```yaml
version: v1

azureMetadata:
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  tenantId: 0f9d7fea-99e8-4768-8672-06a28514f77e
  resourceGroupName: promitor
```

Expected result:

```text
5:2: 'tenantId' has already been specified at line 4, column 2.
```

## Implementation

We could start by altering the `Deserializer` class so that you can describe the
fields you want to deserialize in the subclasses. For example, we would be able
to define the `AzureMetadataDeserializer` like this:

```csharp
public class AzureMetadataDeserializer : Deserializer<AzureMetadataV1>
{
    public AzureMetadataDeserializer()
    {
        MapRequired(metadata => metadata.TenantId);
        MapRequired(metadata => metadata.SubscriptionId);
        MapOptional(metadata => metadata.ResourceGroupName);
    }
}
```

*(I know that the group name isn't optional in the Azure Metadata, but I'm just
using it as an example)*

This means that the Deserializer now knows about the fields that need to be deserialized.
So when we call its `Deserialize()` method, it can set any fields, and then report
any missing required fields. As pseudocode it would look something like this:

```csharp
public TObject Deserialize(YamlMappingNode node, ErrorReporter errorReporter)
{
    var result = new TObject();
    foreach (var child in node.Children)
    {
        if (FieldIsValid(child))
        {
            SetValue(result, child);
        }
        else
        {
            WarnAboutUnknownField(errorReporter, child);
        }
    }

    ReportMissingRequiredFields(errorReporter);

    return result;
}
```

The `ErrorReporter` interface would look something like this:

```csharp
public interface ErrorReporter
{
    bool HasErrors { get; }
    bool HasMessages { get; }
    void ReportError(YamlNode node, string message);
    void ReportWarning(YamlNode node, string message);
    void OutputMessages(TextWriter textWriter);
}
```

The `YamlNode` has information in it about line and column numbers, so we can add
that to the error messages output.

The overall deserialization process would look something like this:

```csharp
var errorReporter = new DefaultErrorReporter();

var configuration = rootDeserializer.Deserialize(yamlNode, errorReporter);

if (errorReporter.HasMessages)
{
    // Output the messages

    if (errorReporter.HasErrors)
    {
        // Exit the application
    }
}
```

### Property Name Suggestions

Because the deserializers know about their supported properties, we can use a
string distance algorithm to figure out possible suggestions based on the actual
name supplied.

For example, if we had the following yaml:

```yaml
tennatId: abc-123
subscriptionId: def-321
resourceGroupId: group
```

We know that the three possible correct names are "tenantId", "subscriptionId",
and "resourceGroupId". We can then use something like a [distance algorithm](https://www.csharpstar.com/csharp-string-distance-algorithm/)
to calculate which of our correct names are close enough to what was supplied for
us to suggest it.
