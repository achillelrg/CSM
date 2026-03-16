# Annexe - Robot et reseau

Cette annexe decrit la brique robot disponible dans le depot ainsi que les contraintes pratiques liees a son utilisation en salle. Elle doit permettre au lecteur de comprendre ce qui est deja fourni par le projet, sans laisser penser que l'integration complete avec le capteur est deja achevee.

Le projet `xARMForm` fournit une base C# exploitable pour le pilotage du robot. On y retrouve les operations de connexion, l'activation du mouvement, la lecture de la pose courante et des articulations, ainsi que plusieurs commandes de deplacement cartesiens ou articulaires. Les primitives telles que `Create`, `EnableMotion`, `GetCurrentPosition`, `GetCurrentJoint`, `MoveTool`, `MoveBase` et `MoveHome` constituent donc deja un socle `Disponible` pour la suite du projet.

Cette base doit toutefois etre lue avec les contraintes d'usage propres a la cellule robotisee. Le pilotage du robot passe par une connexion reseau vers le controleur xArm, ce qui implique d'etre sur le bon acces de salle et de disposer des autorisations pratiques necessaires. Par ailleurs, un seul utilisateur peut etre connecte au robot a un instant donne. Cette contrainte explique en partie pourquoi la validation experimentale ne peut pas etre consideree comme acquise tant qu'aucun essai reel n'a ete documente.

Pour le rendu final, cette annexe devra etre completee par des preuves simples et ciblees : une capture de connexion reussie, une lecture de pose ou d'articulations, puis un exemple de deplacement `MoveTool` sur l'axe retenu. Il n'est pas necessaire d'en faire davantage si ces elements suffisent a montrer que la brique robot a bien ete prise en main.
