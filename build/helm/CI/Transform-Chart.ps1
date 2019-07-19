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

echo 'Changing name of chart'
((Get-Content -path $chartName/Chart.yaml -Raw) -replace 'name: promitor-agent-scraper', 'name: $chartName') | Set-Content -Path $chartName/Chart.yaml

echo 'Changing image tag'
((Get-Content -path $chartName/values.yaml -Raw) -replace '  tag: 1.0.0-preview-8', '  tag: $imageTag') | Set-Content -Path $chartName/values.yaml

echo 'Changing repo name'
((Get-Content -path $chartName/values.yaml -Raw) -replace '  repository: tomkerkhove/promitor-agent-scraper', '  repository: $imageName') | Set-Content -Path $chartName/values.yaml

echo 'Change name of chart in documentation samples'
((Get-Content -path $chartName/README.md -Raw) -replace 'promitor-agent-scraper', 'promitor-agent-scraper-ci') | Set-Content -Path $chartName/README.md