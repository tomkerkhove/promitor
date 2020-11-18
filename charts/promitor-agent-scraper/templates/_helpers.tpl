{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "promitor-agent-scraper.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "promitor-agent-scraper.fullname" -}}
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
{{- define "promitor-agent-scraper.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
    Common labels
*/}}
{{- define "promitor-agent-scraper.labels" -}}
helm.sh/chart: {{ include "promitor-agent-scraper.chart" . }}
{{ include "promitor-agent-scraper.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
    Selector labels
*/}}
{{- define "promitor-agent-scraper.selectorLabels" -}}
app.kubernetes.io/name: {{ include "promitor-agent-scraper.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
    Create secret name based on whether or not user defined it.
*/}}
{{- define "promitor-agent-scraper.secretname" -}}
{{- if .Values.secrets.createSecret -}}
{{ template "promitor-agent-scraper.fullname" . }}
{{- else -}}
{{- printf "%s" .Values.secrets.secretName -}}
{{- end -}}
{{- end -}}

{{/*
    Create service account name based on whether or not user defined it.
*/}}
{{- define "promitor-agent-scraper.serviceaccountname" -}}
{{- if .Values.rbac.serviceAccount.create -}}
{{ template "promitor-agent-scraper.fullname" . }}
{{- else -}}
{{- printf "%s" .Values.rbac.serviceAccount.name -}}
{{- end -}}
{{- end -}}