param(
    [string]$OutputPdf = "Rapport_CSM_Robot_Force.pdf"
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$mdPath = Join-Path $root "Rapport_CSM_Robot_Force.md"
$pdfPath = Join-Path $root $OutputPdf
$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("rapport_csm_robot_force_" + [System.Guid]::NewGuid().ToString("N"))
$htmlPath = Join-Path $tempRoot "Rapport_CSM_Robot_Force.html"

if (-not (Test-Path $mdPath)) {
    throw "Markdown source not found: $mdPath"
}

New-Item -ItemType Directory -Force -Path $tempRoot | Out-Null

function Convert-InlineMarkdown {
    param([string]$Text)

    $encoded = [System.Net.WebUtility]::HtmlEncode($Text)
    $encoded = [regex]::Replace($encoded, '!\[([^\]]*)\]\(([^)]+)\)', '<img alt="$1" src="$2" />')
    $encoded = [regex]::Replace($encoded, '\[([^\]]+)\]\(([^)]+)\)', '<a href="$2">$1</a>')
    $encoded = [regex]::Replace($encoded, '\*\*([^*]+)\*\*', '<strong>$1</strong>')
    $encoded = [regex]::Replace($encoded, '`([^`]+)`', '<code>$1</code>')
    return $encoded
}

function Convert-MarkdownToHtml {
    param([string[]]$Lines)

    $html = New-Object System.Text.StringBuilder
    $inCode = $false
    $inUl = $false
    $inOl = $false
    $paragraph = New-Object System.Collections.Generic.List[string]

    function Flush-Paragraph {
        param($Paragraph, $Builder)
        if ($Paragraph.Count -gt 0) {
            [void]$Builder.AppendLine("<p>$(Convert-InlineMarkdown (($Paragraph -join ' ').Trim()))</p>")
            $Paragraph.Clear()
        }
    }

    function Close-Lists {
        param([ref]$UlState, [ref]$OlState, $Builder)
        if ($UlState.Value) {
            [void]$Builder.AppendLine("</ul>")
            $UlState.Value = $false
        }
        if ($OlState.Value) {
            [void]$Builder.AppendLine("</ol>")
            $OlState.Value = $false
        }
    }

    foreach ($rawLine in $Lines) {
        $line = $rawLine.TrimEnd()

        if ($line -match '^```') {
            Flush-Paragraph $paragraph $html
            Close-Lists ([ref]$inUl) ([ref]$inOl) $html
            if (-not $inCode) {
                [void]$html.AppendLine("<pre><code>")
                $inCode = $true
            }
            else {
                [void]$html.AppendLine("</code></pre>")
                $inCode = $false
            }
            continue
        }

        if ($inCode) {
            [void]$html.AppendLine([System.Net.WebUtility]::HtmlEncode($line))
            continue
        }

        if ([string]::IsNullOrWhiteSpace($line)) {
            Flush-Paragraph $paragraph $html
            Close-Lists ([ref]$inUl) ([ref]$inOl) $html
            continue
        }

        if ($line -match '^(#{1,6})\s+(.*)$') {
            Flush-Paragraph $paragraph $html
            Close-Lists ([ref]$inUl) ([ref]$inOl) $html
            $level = $matches[1].Length
            $content = Convert-InlineMarkdown $matches[2]
            [void]$html.AppendLine("<h$level>$content</h$level>")
            continue
        }

        if ($line -match '^\-\s+(.*)$') {
            Flush-Paragraph $paragraph $html
            if (-not $inUl) {
                Close-Lists ([ref]$inUl) ([ref]$inOl) $html
                [void]$html.AppendLine("<ul>")
                $inUl = $true
            }
            [void]$html.AppendLine("<li>$(Convert-InlineMarkdown $matches[1])</li>")
            continue
        }

        if ($line -match '^\d+\.\s+(.*)$') {
            Flush-Paragraph $paragraph $html
            if (-not $inOl) {
                Close-Lists ([ref]$inUl) ([ref]$inOl) $html
                [void]$html.AppendLine("<ol>")
                $inOl = $true
            }
            [void]$html.AppendLine("<li>$(Convert-InlineMarkdown $matches[1])</li>")
            continue
        }

        $paragraph.Add($line)
    }

    Flush-Paragraph $paragraph $html
    Close-Lists ([ref]$inUl) ([ref]$inOl) $html

    if ($inCode) {
        [void]$html.AppendLine("</code></pre>")
    }

    return $html.ToString()
}

$markdownLines = Get-Content -Path $mdPath -Encoding UTF8
$bodyHtml = Convert-MarkdownToHtml $markdownLines

$style = @"
body {
  font-family: "Segoe UI", Arial, sans-serif;
  margin: 28px 42px;
  color: #1f1f1f;
  line-height: 1.5;
  font-size: 12pt;
}
h1, h2, h3, h4 {
  color: #0f3557;
  page-break-after: avoid;
}
h1 {
  font-size: 24pt;
  border-bottom: 2px solid #0f3557;
  padding-bottom: 6px;
}
h2 {
  margin-top: 26px;
  font-size: 18pt;
}
h3 {
  margin-top: 18px;
  font-size: 14pt;
}
p {
  text-align: justify;
}
ul, ol {
  margin-top: 4px;
  margin-bottom: 10px;
}
pre {
  background: #f4f6f8;
  border: 1px solid #d4dbe2;
  border-radius: 6px;
  padding: 12px;
  overflow-x: auto;
  white-space: pre-wrap;
  font-size: 10pt;
}
code {
  font-family: Consolas, "Courier New", monospace;
}
img {
  max-width: 100%;
  border: 1px solid #c8d0d8;
  border-radius: 6px;
  margin: 10px 0 18px 0;
}
a {
  color: #0a5ca8;
  text-decoration: none;
}
@page {
  size: A4;
  margin: 16mm;
}
"@

$html = @"
<!doctype html>
<html lang="fr">
<head>
  <meta charset="utf-8" />
  <title>Rapport CSM Robot + Force</title>
  <style>
$style
  </style>
</head>
<body>
$bodyHtml
</body>
</html>
"@

Set-Content -Path $htmlPath -Value $html -Encoding UTF8

try {
    $browserCandidates = @(
        "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
        "C:\Program Files\Microsoft\Edge\Application\msedge.exe",
        "C:\Program Files\Google\Chrome\Application\chrome.exe"
    )
    $browser = $browserCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1

    if (-not $browser) {
        throw "No compatible browser found for PDF generation."
    }

    $uri = [System.Uri]::new($htmlPath).AbsoluteUri
    & $browser --headless=new --disable-gpu --print-to-pdf="$pdfPath" --print-to-pdf-no-header $uri | Out-Null

    if (-not (Test-Path $pdfPath)) {
        throw "PDF generation failed: $pdfPath was not created."
    }

    Write-Host "PDF generated: $pdfPath"
}
finally {
    Remove-Item -Recurse -Force -Path $tempRoot -ErrorAction SilentlyContinue
}
