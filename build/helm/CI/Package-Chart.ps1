Param(
    [String]
    [Parameter(Mandatory = $true)] $chartName = $(throw "Chart name is required"),
    [String]
    [Parameter(Mandatory = $true)] $chartVersion = $(throw "Version of the chart is required")
)

echo 'Creating output folder'
mkdir output/

echo 'Packaging chart'
helm package $chartName/ --app-version $chartVersion --version $chartVersion --destination output/