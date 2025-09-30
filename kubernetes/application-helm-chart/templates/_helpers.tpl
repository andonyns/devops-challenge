{{- define "backend.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end }}

{{- define "backend.fullname" -}}
{{- printf "%s" (include "backend.name" .) | trunc 63 | trimSuffix "-" -}}
{{- end }}