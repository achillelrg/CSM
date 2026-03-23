param(
    [string]$OutputZip = "Livrable_CSM_Robot_Force.zip"
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$pdfCandidates = @(
    (Join-Path $root "livrables\Rapport_CSM_Robot_Force.pdf"),
    (Join-Path $root "Rapport_CSM_Robot_Force.pdf")
)

$reportPdf = $pdfCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $reportPdf) {
    throw "Report PDF not found. Expected one of: $($pdfCandidates -join ', ')"
}

$sourceCodeRoot = Join-Path $root "demo\csharp_studio\RobotForceIntegration"
if (-not (Test-Path $sourceCodeRoot)) {
    throw "Source folder not found: $sourceCodeRoot"
}

$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("livrable_csm_" + [System.Guid]::NewGuid().ToString("N"))
$deliverableRoot = Join-Path $tempRoot "Livrable_CSM_Robot_Force"
$codeOut = Join-Path $deliverableRoot "code\RobotForceIntegration"

New-Item -ItemType Directory -Path $codeOut -Force | Out-Null

$null = robocopy $sourceCodeRoot $codeOut /E /XD bin obj .vs /XF *.user *.suo *.userosscache *.sln.docstates /NFL /NDL /NJH /NJS /NC /NS
if ($LASTEXITCODE -ge 8) {
    throw "robocopy failed while copying source code."
}

Copy-Item -Path $reportPdf -Destination (Join-Path $deliverableRoot "Rapport_CSM_Robot_Force.pdf") -Force

$readmePath = Join-Path $deliverableRoot "README_INSTALLATION_PROF.md"
$readmeContent = @"
# Livrable CSM Robot + Force

Ce livrable contient uniquement :
- `Rapport_CSM_Robot_Force.pdf`
- le code fonctionnel dans `code/RobotForceIntegration`

## Mise en place rapide (prof)

1. Ouvrir `code/RobotForceIntegration/RobotForceIntegration.csproj` dans Visual Studio 2022.
2. Verifier que le workload **.NET desktop development** est installe.
3. Verifier que le **targeting pack .NET Framework 4.8** est present.
4. Lancer en `Debug|Any CPU` (la cible interne du projet est `x64`).

## Notes execution

- `xarm.dll` est deja inclus dans `code/RobotForceIntegration/lib/xarm.dll`.
- Configurer l'IP robot et le port COM capteur dans l'IHM avant test.
"@
Set-Content -Path $readmePath -Value $readmeContent -Encoding UTF8

$zipPath = Join-Path $root $OutputZip
if (Test-Path $zipPath) {
    Remove-Item -Path $zipPath -Force
}

Compress-Archive -Path (Join-Path $deliverableRoot "*") -DestinationPath $zipPath -CompressionLevel Optimal
Write-Host "Livrable generated: $zipPath"

Remove-Item -Path $tempRoot -Recurse -Force -ErrorAction SilentlyContinue
