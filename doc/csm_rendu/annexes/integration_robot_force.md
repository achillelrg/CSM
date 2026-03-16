# Annexe - Integration Robot + Force

Cette annexe a pour objet de decrire la cible d'integration du projet et d'expliciter l'ecart entre cette cible et l'etat reel du depot. Elle joue un role important dans le rendu, car elle permet de montrer que l'architecture technique est comprise, meme si l'application finale n'est pas encore totalement finalisee.

La logique attendue par le sujet est relativement claire. Une mesure de force doit etre demandee au capteur, puis la valeur correspondante doit etre extraite du flux serie. Cette mesure doit ensuite etre rapprochee de la position courante du robot afin de calculer une correction, typiquement un `Delta Z`, qui sera appliquee par une commande de type `MoveTool`. L'ensemble des donnees utiles doit enfin etre conserve afin de pouvoir produire un fichier CSV exploitable lors de l'analyse des essais.

Dans le depot actuel, cette chaine complete n'est pas encore integree dans une application unique. La base `RS232-PC` reste centree sur la liaison serie et l'acquisition, tandis que `xARMForm` reste centree sur le pilotage robot. L'integration Robot + Force est donc encore `A completer`. Il est important que le rapport le dise explicitement afin d'eviter toute ambiguite sur l'etat reel du projet.

Le travail restant est bien identifie. Il consiste a reprendre les classes de pilotage robot dans la base principale, a raccorder les commandes de mouvement a l'IHM commune, a synchroniser la lecture capteur et la lecture robot, puis a centraliser les donnees de force et de position dans une logique d'essai coherente. Cette annexe devra ensuite etre completee par un schema simple et, si possible, par une capture de l'application integree lorsque celle-ci sera disponible.
