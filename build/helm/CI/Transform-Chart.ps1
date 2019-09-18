Param(
    [String]
    [Parameter(Mandatory = $true)] $chartName = $(throw "Chart name is required"),
    [String]
    [Parameter(Mandatory = $true)] $imageName = $(throw "Image name is required"),
    [String]
    [Parameter(Mandatory = $true)] $imageTag = $(throw "Image tag is required")
)

echo 'Copying Chart folder'
cp promitor-agent-scraper/ $chartName/ -r

echo 'Determining image version'
$imageVersion = "$imageTag".ToLower()
echo 'Image version is $imageVersion'

echo 'Changing name of chart'
((Get-Content -path $chartName/Chart.yaml -Raw) -replace 'promitor-agent-scraper', $chartName) | Set-Content -Path $chartName/Chart.yaml

echo 'Changing image tag'
((Get-Content -path $chartName/values.yaml -Raw) -replace '1.0.0', $imageVersion) | Set-Content -Path $chartName/values.yaml

echo 'Changing repo name'
((Get-Content -path $chartName/values.yaml -Raw) -replace 'tomkerkhove/promitor-agent-scraper', $imageName) | Set-Content -Path $chartName/values.yaml

echo 'Change name of chart in documentation samples'
((Get-Content -path $chartName/README.md -Raw) -replace 'promitor-agent-scraper', 'promitor-agent-scraper-ci') | Set-Content -Path $chartName/README.md

echo 'Outputting transformed content'
Get-Content -path $chartName/Chart.yaml -Raw
Get-Content -path $chartName/values.yaml -Raw
Get-Content -path $chartName/README.md -Raw