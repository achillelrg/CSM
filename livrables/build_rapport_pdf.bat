@echo off
setlocal EnableExtensions

set "ROOT=%~dp0"
set "TEX=%ROOT%Rapport_CSM_Robot_Force.tex"
set "OUTDIR=%ROOT:~0,-1%"
set "LATEXMK="
set "PDFLATEX="

if not exist "%TEX%" (
  echo [ERREUR] Fichier source introuvable : "%TEX%"
  exit /b 1
)

if not exist "%OUTDIR%" mkdir "%OUTDIR%"

where latexmk >nul 2>nul && set "LATEXMK=latexmk"
where pdflatex >nul 2>nul && set "PDFLATEX=pdflatex"

if not defined LATEXMK if exist "C:\Program Files\MiKTeX\miktex\bin\x64\latexmk.exe" set "LATEXMK=C:\Program Files\MiKTeX\miktex\bin\x64\latexmk.exe"
if not defined PDFLATEX if exist "C:\Program Files\MiKTeX\miktex\bin\x64\pdflatex.exe" set "PDFLATEX=C:\Program Files\MiKTeX\miktex\bin\x64\pdflatex.exe"

if not defined LATEXMK if exist "C:\texlive\2025\bin\windows\latexmk.exe" set "LATEXMK=C:\texlive\2025\bin\windows\latexmk.exe"
if not defined PDFLATEX if exist "C:\texlive\2025\bin\windows\pdflatex.exe" set "PDFLATEX=C:\texlive\2025\bin\windows\pdflatex.exe"

if not defined LATEXMK if exist "C:\texlive\2024\bin\windows\latexmk.exe" set "LATEXMK=C:\texlive\2024\bin\windows\latexmk.exe"
if not defined PDFLATEX if exist "C:\texlive\2024\bin\windows\pdflatex.exe" set "PDFLATEX=C:\texlive\2024\bin\windows\pdflatex.exe"

if defined LATEXMK (
  echo [INFO] Compilation avec latexmk...
  "%LATEXMK%" -pdf -interaction=nonstopmode -halt-on-error -outdir="%OUTDIR%" "%TEX%"
  if errorlevel 1 (
    echo [ERREUR] La compilation LaTeX a echoue avec latexmk.
    exit /b 1
  )
  echo [OK] PDF genere : "%OUTDIR%\Rapport_CSM_Robot_Force.pdf"
  exit /b 0
)

if defined PDFLATEX (
  echo [INFO] Compilation avec pdflatex...
  "%PDFLATEX%" -interaction=nonstopmode -halt-on-error -output-directory="%OUTDIR%" "%TEX%"
  if errorlevel 1 (
    echo [ERREUR] Premier passage pdflatex en echec.
    exit /b 1
  )
  "%PDFLATEX%" -interaction=nonstopmode -halt-on-error -output-directory="%OUTDIR%" "%TEX%"
  if errorlevel 1 (
    echo [ERREUR] Second passage pdflatex en echec.
    exit /b 1
  )
  echo [OK] PDF genere : "%OUTDIR%\Rapport_CSM_Robot_Force.pdf"
  exit /b 0
)

echo [ERREUR] Aucun compilateur LaTeX detecte.
echo [INFO] Installez MiKTeX ou TeX Live, puis relancez :
echo [INFO]   "%~nx0"
echo [INFO] Le script cherche d'abord latexmk, puis pdflatex.
exit /b 1
