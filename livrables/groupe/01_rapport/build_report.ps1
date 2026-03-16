param(
    [string]$TexFile = "csm_rapport_groupe.tex"
)

$ErrorActionPreference = "Stop"
$here = Split-Path -Parent $MyInvocation.MyCommand.Path

Push-Location $here
try
{
    & pdflatex -interaction=nonstopmode -halt-on-error $TexFile | Out-Host
    & pdflatex -interaction=nonstopmode -halt-on-error $TexFile | Out-Host
}
finally
{
    Pop-Location
}
