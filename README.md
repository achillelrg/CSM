# CSM Robot + Force

Application WinForms C# pour le TP CSM:

- connexion capteur de force sur liaison serie (`115200 / 8N1`)
- connexion robot xArm sur le reseau
- jog manuel en repere TOOL
- mode `test mecanique` avec timer et export CSV
- mode `asservissement en force Z` avec PID externe

## Structure utile

- `csharp_studio.sln`: solution Visual Studio a ouvrir
- `RS232-PC/`: projet principal
- `RS232-PC/Interop/`: wrapper C# xArm embarque dans le projet
- `RS232-PC/Native/xarm.dll`: DLL native fournie pour le pilotage xArm

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
4. Asservissement:
   - regler seuil, fenetre Z, setpoint et gains
   - lancer `Force control`

## Remarque compilation

Sur certaines machines, le pack de ciblage `.NET Framework 4.8` doit etre installe pour compiler via MSBuild hors Visual Studio.
