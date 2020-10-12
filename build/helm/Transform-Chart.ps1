Param(
    [String]
    [Parameter(Mandatory = $true)] $chartName = $(throw "Chart name is required"),
    [String]
    [Parameter(Mandatory = $true)] $transformedChartName = $(throw "Chart name of the transformed output is required"),
    [String]
    [Parameter(Mandatory = $true)] $imageName = $(throw "Image name is required"),
    [String]
    [Parameter(Mandatory = $true)] $imageTag = $(throw "Image tag is required")
)

echo "Copying Chart folder"
cp $chartName/ $transformedChartName/ -r

echo "Determining image version"
$imageVersion = "$imageTag".ToLower()
echo "Image version is $imageVersion"

echo "Changing name of chart to $transformedChartName"
((Get-Content -path $transformedChartName/Chart.yaml -Raw) -replace $chartName, $transformedChartName) | Set-Content -Path $transformedChartName/Chart.yaml

echo "Changing image tag to $imageVersion"
((Get-Content -path $transformedChartName/values.yaml -Raw) -replace 'latest', $imageVersion) | Set-Content -Path $transformedChartName/values.yaml

echo "Changing repo name to $imageName"
((Get-Content -path $transformedChartName/values.yaml -Raw) -replace 'tomkerkhove/' + $chartName, $imageName) | Set-Content -Path $transformedChartName/values.yaml

echo "Change name of chart in README to $transformedChartName"
((Get-Content -path $transformedChartName/README.md -Raw) -replace $chartName, $transformedChartName) | Set-Content -Path $transformedChartName/README.md

echo "Change version of image in README to $imageVersion"
((Get-Content -path $transformedChartName/README.md -Raw) -replace 'latest', $imageVersion) | Set-Content -Path $transformedChartName/README.md

echo "Outputting transformed content"
Get-Content -path $transformedChartName/Chart.yaml -Raw
Get-Content -path $transformedChartName/values.yaml -Raw
Get-Content -path $transformedChartName/README.md -Raw