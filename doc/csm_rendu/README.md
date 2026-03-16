# Rendu Markdown CSM

Ce dossier contient la base du rendu de groupe en Markdown.

## Fichiers principaux

- `rapport_principal.md`: document principal
- `annexes/`: annexes Markdown et techniques
- `checklist_rendu.md`: controle de conformite avant depot
- `build_pdf.ps1`: script de compilation Markdown -> PDF

## Compilation PDF

Depuis PowerShell:

```powershell
cd C:\WORKSPACE\CSM\csharp_studio\doc\csm_rendu
.\build_pdf.ps1
```

Le PDF est genere dans:

```text
output/pdf/rapport_csm_groupe.pdf
```

## Notes

- Le script combine le rapport principal et les annexes Markdown.
- Les annexes techniques non Markdown (`.txt`, `.csv`) restent des pieces jointes separees du rendu.
- Les fichiers temporaires de compilation sont places dans `tmp/pdfs/`.
- Outils necessaires sur la machine:
  - `pandoc`
  - `pdflatex` via MiKTeX ou TeX Live
