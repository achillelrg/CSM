# Livrables CSM

Cette arborescence separe clairement:

- les livrables de groupe, versionnes et pousses sur GitHub
- les livrables perso, conserves localement et ignores par Git

## Regles

- `livrables/groupe/` contient uniquement ce qui sert au rendu commun.
- `livrables/perso/` est reserve aux notes, brouillons et essais individuels.
- Ne jamais pousser de donnees sensibles, d'acces reseau ou de brouillons non valides.
- Le rapport de groupe est pilote depuis `livrables/groupe/01_rapport/`.

## Demarrage

Pour creer votre espace perso local:

```powershell
.\livrables\init_perso.ps1 -Name prenom_nom
```

Le dossier sera cree sous `livrables/perso/prenom_nom/` avec des sous-dossiers de travail.
