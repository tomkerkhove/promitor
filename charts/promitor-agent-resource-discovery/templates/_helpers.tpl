{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "promitor-agent-resource-discovery.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "promitor-agent-resource-discovery.fullname" -}}
{{- if .Values.fullnameOverride -}}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- $name := default .Chart.Name .Values.nameOverride -}}
{{- if contains $name .Release.Name -}}
{{- .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
{{- end -}}
{{- end -}}

{{/*
    Create chart name and version as used by the chart label.
*/}}
{{- define "promitor-agent-resource-discovery.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
    Create secret name based on whether or not user defined it.
*/}}
{{- define "promitor-agent-resource-discovery.secretname" -}}
{{- if .Values.secrets.createSecret -}}
{{ template "promitor-agent-resource-discovery.fullname" . }}
{{- else -}}
{{- printf "%s" .Values.secrets.secretName -}}
{{- end -}}
{{- end -}}

{{/*
    Create service account name based on whether or not user defined it.
*/}}
{{- define "promitor-agent-resource-discovery.serviceaccountname" -}}
{{- if .Values.rbac.serviceAccount.create -}}
{{ template "promitor-agent-resource-discovery.fullname" . }}
{{- else -}}
{{- printf "%s" .Values.rbac.serviceAccount.name -}}
{{- end -}}
{{- end -}}