# Annexe - Tests et validation

## Objet

Cette annexe sert de tableau de bord de validation experimentale.

## 1. Tests deja identifiables dans les sources

Statut: `Disponible`

- ouverture / fermeture du port serie
- acquisition periodique RS232
- sauvegarde CSV
- connexion robot
- lecture pose / joints
- deplacements `MoveTool`
- timer de demonstration cote robot

## 2. Tests a realiser en seance

Statut: `A valider`

- capteur avec emulateur
- capteur reel
- robot reel en reseau
- premiers deplacements en repere TOOL
- integration force / position Z
- export CSV d'essais reels
- test mecanique
- asservissement en force

## 3. Tableau de suivi

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

## 4. Regle de redaction

Quand un test n'est pas encore realise:

- ne pas le cocher comme valide
- indiquer `A completer` ou `A valider`
- reserver la colonne preuve pour plus tard
