# Annexe - Tests et validation

Cette annexe sert de support au suivi experimental du projet. Elle ne remplace pas le commentaire redige dans le rapport principal, mais elle fournit un cadre simple pour distinguer ce qui est deja observable dans les sources, ce qui doit encore etre essaye en seance et ce qui devra etre prouve par des captures ou des fichiers de sortie.

A ce jour, plusieurs fonctions peuvent deja etre considerees comme visibles dans les briques existantes. C'est le cas de l'ouverture et de la fermeture du port serie, de l'acquisition periodique cote `RS232-PC`, de la sauvegarde CSV, de la connexion robot, de la lecture de pose et d'articulations, ainsi que des deplacements `MoveTool` et du timer de demonstration cote robot. Ces points constituent une base technique utile, mais ils ne remplacent pas une validation experimentale sur le materiel reel.

La campagne d'essais a mener en seance doit donc couvrir, au minimum, le capteur avec emulateur, le capteur reel, la connexion reseau au robot, les premiers deplacements en repere TOOL, l'integration force / position sur Z, l'export CSV d'essais reels, puis les scenarios de test mecanique et d'asservissement. Le tableau ci-dessous est prevu pour consigner ces verifications au fur et a mesure de l'avancement de l'equipe.

| Test | Statut | Date | Operateur | Preuve | Commentaire |
| --- | --- | --- | --- | --- | --- |
| Port serie ouvert | Disponible | [A completer] | [A completer] | [Capture] | |
| Lecture message `Reading` | Disponible | [A completer] | [A completer] | [Capture] | |
| Export CSV | Disponible | [A completer] | [A completer] | [CSV] | |
| Connexion xArm | A valider | [A completer] | [A completer] | [Capture] | |
| Lecture pose | A valider | [A completer] | [A completer] | [Capture] | |
| `MoveTool` | A valider | [A completer] | [A completer] | [Capture] | |
| Integration Robot + Force | A completer | [A completer] | [A completer] | [Capture] | |
| Test mecanique | A completer | [A completer] | [A completer] | [CSV] | |
| Asservissement en force | A completer | [A completer] | [A completer] | [CSV / Video] | |

Lorsqu'un essai n'a pas encore ete realise, il est preferable de le laisser `A completer` ou `A valider` plutot que de le formuler de maniere ambigue. Cette annexe doit rester un outil de suivi sobre et factuel, pas un tableau rempli pour donner artificiellement l'impression que tout a deja ete teste.
