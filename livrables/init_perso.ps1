param(
    [Parameter(Mandatory = $true)]
    [string]$Name
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$personalRoot = Join-Path $root "perso"
$userRoot = Join-Path $personalRoot $Name

$folders = @(
    $userRoot,
    (Join-Path $userRoot "notes"),
    (Join-Path $userRoot "brouillons"),
    (Join-Path $userRoot "captures_brutes"),
    (Join-Path $userRoot "csv_bruts")
)

foreach ($folder in $folders)
{
    New-Item -ItemType Directory -Force -Path $folder | Out-Null
}

$readmePath = Join-Path $userRoot "README_LOCAL.txt"
if (-not (Test-Path $readmePath))
{
    @"
Ce dossier est personnel et ignore par Git.

Utilisation recommandee:
- notes/: notes de seance et hypotheses
- brouillons/: texte individuel avant validation groupe
- captures_brutes/: captures non retenues pour le rendu
- csv_bruts/: essais bruts ou non valides

Ne rien deplacer vers livrables/groupe tant que le contenu n'est pas relu.
"@ | Set-Content -Path $readmePath -Encoding ASCII
}

Write-Host "Personal workspace ready: $userRoot"
