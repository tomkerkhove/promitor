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
