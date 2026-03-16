param(
    [string]$OutputName = "rapport_csm_groupe.pdf"
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$tmpRoot = Join-Path $root "tmp\pdfs"
$outputRoot = Join-Path $root "output\pdf"
$combinedMarkdown = Join-Path $tmpRoot "rapport_complet.md"
$latexFile = Join-Path $tmpRoot "rapport_complet.tex"
$tempPdf = Join-Path $tmpRoot "rapport_complet.pdf"
$outputPdf = Join-Path $outputRoot $OutputName

$markdownFiles = @(
    (Join-Path $root "rapport_principal.md"),
    (Join-Path $root "annexes\ihm.md"),
    (Join-Path $root "annexes\capteur_serie.md"),
    (Join-Path $root "annexes\robot_reseau.md"),
    (Join-Path $root "annexes\integration_robot_force.md"),
    (Join-Path $root "annexes\tests_validation.md"),
    (Join-Path $root "annexes\extraits_code.md")
)

New-Item -ItemType Directory -Force -Path $tmpRoot | Out-Null
New-Item -ItemType Directory -Force -Path $outputRoot | Out-Null

$builder = New-Object System.Text.StringBuilder
$builder.AppendLine('% Rapport CSM combine') | Out-Null
$builder.AppendLine() | Out-Null

for ($i = 0; $i -lt $markdownFiles.Count; $i++)
{
    $path = $markdownFiles[$i]
    if (-not (Test-Path $path))
    {
        throw "Missing markdown source: $path"
    }

    if ($i -gt 0)
    {
        $builder.AppendLine() | Out-Null
        $builder.AppendLine('\newpage') | Out-Null
        $builder.AppendLine() | Out-Null
    }

    $content = Get-Content -Raw -Path $path
    $builder.AppendLine($content.Trim()) | Out-Null
    $builder.AppendLine() | Out-Null
}

$builder.ToString() | Set-Content -Path $combinedMarkdown -Encoding UTF8

Push-Location $tmpRoot
try
{
    & pandoc `
        $combinedMarkdown `
        "--from=markdown" `
        "--to=latex" `
        "--standalone" `
        "--table-of-contents" `
        "--toc-depth=3" `
        "--number-sections" `
        "--metadata=title:Rapport CSM Robot + Force" `
        "--metadata=author:Equipe CSM" `
        "--metadata=date:$(Get-Date -Format 'yyyy-MM-dd')" `
        "--variable=geometry:margin=2.2cm" `
        "-o" `
        $latexFile

    if ($LASTEXITCODE -ne 0)
    {
        throw "Pandoc failed with exit code $LASTEXITCODE."
    }

    & pdflatex -interaction=nonstopmode -halt-on-error $latexFile | Out-Host
    if ($LASTEXITCODE -ne 0)
    {
        throw "pdflatex failed on first pass with exit code $LASTEXITCODE."
    }

    & pdflatex -interaction=nonstopmode -halt-on-error $latexFile | Out-Host
    if ($LASTEXITCODE -ne 0)
    {
        throw "pdflatex failed on second pass with exit code $LASTEXITCODE."
    }
}
finally
{
    Pop-Location
}

if (-not (Test-Path $tempPdf))
{
    throw "Expected PDF was not generated: $tempPdf"
}

Copy-Item -Force -Path $tempPdf -Destination $outputPdf

Write-Host "PDF generated: $outputPdf"
