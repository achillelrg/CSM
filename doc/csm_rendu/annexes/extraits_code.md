# Annexe - Extraits de code a commenter

Cette annexe ne contient pas encore les captures de code finales. Son role est de preparer une selection pertinente des blocs qui meriteront d'etre montres et commentes dans le rendu. L'objectif est d'eviter, au moment de finaliser le rapport, une accumulation d'extraits trop longs ou redondants.

Pour la brique capteur `RS232-PC`, les points les plus pertinents a illustrer sont l'ouverture du port serie, la reception asynchrone, le traitement du flux recu, la logique d'acquisition periodique et l'export CSV. Dans l'etat actuel du depot, les methodes `btOpen_Click`, `serialport_DataReceived`, `processReceivedChunk`, `acquisitionTimer_Tick` et `btSave_Click` constituent de bons candidats pour des captures courtes et commentees.

Pour la brique robot `xARMForm`, il sera plus utile de retenir quelques points structurants plutot que de multiplier les extraits. La creation de la connexion, l'activation du mouvement, la lecture de la pose, l'usage de `MoveTool` et le timer de demonstration devraient suffire. Les methodes `ButtonCreateARM_Click`, `ButtonMotionARM_Click`, `ButtonGetPosition_Click`, `ButtonMoveTCP_Click` et `timerCMD_Tick` couvrent deja bien ce besoin.

Le wrapper robot lui-meme pourra etre illustre de facon tres ciblee, par exemple a travers `Create`, `EnableMotion`, `GetCurrentPosition` et `MoveTool`. Lorsque l'application integree existera, cette annexe devra alors etre completee par les blocs qui feront le lien entre la mesure de force, le calcul du `Delta Z`, la commande robot et l'enregistrement des donnees d'essai.
