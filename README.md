# CSM Robot + Force

Application WinForms C# pour le TP CSM:

- connexion capteur de force sur liaison serie (`115200 / 8N1`)
- connexion robot xArm sur le reseau
- reprise des commandes principales de l'interface xARM d'origine dans l'onglet `xARM Base`
- jog manuel en repere TOOL
- mode `guidage manuel` a effort nul sur TOOL Z
- mode `test mecanique` avec timer et export CSV
- mode `asservissement en force Z` avec PID externe

## Structure utile

- `csharp_studio.sln`: solution Visual Studio a ouvrir
- `RS232-PC/`: projet principal
- `RS232-PC/Interop/`: wrapper C# xArm embarque dans le projet
- `RS232-PC/Native/xarm.dll`: DLL native fournie pour le pilotage xArm
- `TP_Utilisation_CSM.md`: guide TP complet (installation, dependances, tests, CSV)
- `livrables/`: separation entre livrables de groupe et espaces perso locaux

## Livrables

- `livrables/groupe/`: contenu versionne et pousse sur GitHub
- `livrables/groupe/01_rapport/`: source LaTeX du rapport et PDF compile
- `livrables/groupe/02_captures/`: captures retenues
- `livrables/groupe/03_csv_exemples/`: CSV exemples pour le rendu
- `livrables/groupe/04_annexes/`: annexes techniques
- `livrables/perso/`: espace local ignore par Git pour les notes, brouillons et essais personnels

Pour creer votre dossier perso local:

```powershell
.\livrables\init_perso.ps1 -Name prenom_nom
```

## Hypothese robot

Le projet est regle pour un **xArm6 (6 axes)**.
L'application n'implemente pas de modele inverse externe. Elle pilote le robot via les primitives du controleur (`MoveTool`) et une loi externe approchee sur `Delta Z`.

## Ouvrir et lancer

1. Ouvrir `csharp_studio.sln` dans Visual Studio.
2. Selectionner la configuration `Debug | x64`.
3. Lancer `RS232-PC`.

## Tests conseilles

1. Capteur seul:
   - ouvrir le port COM
   - verifier le boot dans l'onglet `Capteur`
   - cliquer sur `Measure (M)`
2. Robot seul:
   - entrer l'IP du robot
   - `Connect`
   - `Enable motion`
   - tester `+X/-X`, `+Y/-Y`, `+Z/-Z`
3. Integration:
   - choisir un fichier CSV
   - lancer `Mechanical test`
   - verifier remplissage du tableau
4. Guidage manuel:
   - lancer `Start hand guiding`
   - pousser ou tirer legerement sur le capteur
   - verifier que le robot suit sur Z pour reduire l'effort
5. Asservissement:
   - regler seuil, fenetre Z, setpoint et gains
   - lancer `Force control`

## Remarque compilation

Sur certaines machines, le pack de ciblage `.NET Framework 4.8` doit etre installe pour compiler via MSBuild hors Visual Studio.
