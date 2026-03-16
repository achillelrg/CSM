# Annexe - Extraits de code a commenter

## Objet

Cette annexe ne copie pas encore le code final. Elle liste les blocs les plus pertinents a capturer et commenter dans le rendu final.

## 1. Brique capteur `RS232-PC`

Blocs a capturer plus tard:

- `btOpen_Click` dans `demo/csharp_studio/RS232-PC/MainForm.cs`
- `serialport_DataReceived`
- `processReceivedChunk`
- `acquisitionTimer_Tick`
- `btSave_Click`

Commentaires attendus:

- ouverture et parametrage du port
- reception asynchrone
- extraction de la valeur de force
- acquisition periodique
- sauvegarde CSV

## 2. Brique robot `xARMForm`

Blocs a capturer plus tard:

- `ButtonCreateARM_Click` dans `demo/csharp_studio/xARMForm/Form1.cs`
- `ButtonMotionARM_Click`
- `ButtonGetPosition_Click`
- `ButtonMoveTCP_Click`
- `timerCMD_Tick`

Commentaires attendus:

- creation de la connexion
- activation du mouvement
- lecture de la pose
- utilisation de `MoveTool`
- demonstration par timer

## 3. Wrapper robot

Blocs a capturer plus tard:

- `Create` dans `demo/csharp_studio/xARMForm/Robot.cs`
- `EnableMotion`
- `GetCurrentPosition`
- `MoveTool`

## 4. Integration finale

Statut: `A completer`

Quand l'application integree existera, ajouter:

- le bloc de couplage Robot / Force
- le calcul du `Delta Z`
- la logique de test mecanique
- la logique d'asservissement
