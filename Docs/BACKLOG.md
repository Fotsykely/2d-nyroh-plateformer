# Cahier des charges — Nyroh MVP

> Metroidvania 2D — Références : *Hollow Knight / Silksong*, *9 Sols*
> Engine : Unity 2D (URP) — Architecture : ScriptableObjects + Event Channels
> Solo dev — Art réalisé en interne

---

## 1. Vision du produit

Nyroh est un Metroidvania 2D à combat précis. Le joueur explore un monde interconnecté, débloque des capacités qui ouvrent de nouvelles zones, et affronte des ennemis aux patterns distincts.

**Proposition de valeur MVP :** une boucle jouable complète — se déplacer, combattre, mourir, réapparaître au checkpoint, progresser grâce à une capacité débloquée.

---

## 2. Périmètre MVP

### Inclus

- Mouvement & saut du joueur (déjà implémenté)
- Système de santé & dégâts
- Attaque de base (slash)
- Un ennemi patrouilleur
- Checkpoint & respawn
- Transition entre 2 salles
- Une capacité débloquable (double saut)

### Exclus (backlog futur)

- Parry / counter
- Dash, wall jump
- Boss fight
- Map UI progressive
- Sons & musique
- Art final (polish visuel post-MVP)

### Stratégie art solo

> **Placeholders d'abord.** Chaque sprint utilise des formes colorées ou des sprites temporaires pour valider la mécanique. L'art final arrive après la validation gameplay.

| Phase | Art | Objectif |
|-------|-----|----------|
| MVP | Placeholder (rectangles colorés, sprites basiques) | Valider le gameplay sans être bloqué par l'art |
| Post-MVP | Pixel art final dans Aseprite / LibreSprite | Polisher l'identité visuelle |

---

## 3. Backlog Agile

### Légende

| Statut | Signification |
|--------|--------------|
| `[ ]`  | À faire |
| `[~]`  | En cours |
| `[x]`  | Terminé |

---

### Sprint 0 — Socle *(déjà livré)*

| # | Description | Fichiers | Statut |
|---|-------------|----------|--------|
| S0-1 | Mouvement + saut (coyote time, jump buffer, variable gravity) | `PlayerController.cs`, `CharacterData.cs` | `[x]` |
| S0-2 | Système d'Event Channels SO | `GameEvent.cs`, `GameEventListener.cs` | `[x]` |

**Art S0 — Placeholder**

| # | Asset | Format | Outil | Statut |
|---|-------|--------|-------|--------|
| A0-1 | Sprite player placeholder (rectangle blanc avec indicateur direction) | PNG 32×32 | Unity Sprite Editor | `[x]` |
| A0-2 | Tileset sol placeholder (carré gris) | PNG 16×16 | Unity Sprite Editor | `[x]` |

---

### Sprint 1 — Combat & Santé

**Objectif :** le joueur peut se battre et mourir.

**Tech**

| # | User Story | Acceptance Criteria | Fichiers | Statut |
|---|------------|---------------------|----------|--------|
| S1-1 | En tant que joueur, j'ai une barre de vie et je meurs quand elle atteint 0 | PV visibles, mort déclenche `OnPlayerDied` | `HealthData.cs`, `HealthController.cs`, `OnPlayerDied.asset` | `[ ]` |
| S1-2 | En tant que joueur, je peux attaquer (slash) au sol et en l'air | Animation + hitbox active pendant le slash | `AttackController.cs` | `[ ]` |
| S1-3 | En tant que joueur, mes attaques infligent des dégâts | Les PV d'une cible baissent au contact de la hitbox | `DamageSystem.cs` | `[ ]` |
| S1-4 | En tant que joueur, je suis invincible brièvement après avoir pris un coup (iframes) | Pas de dégâts en double pendant la durée iframes | `HealthController.cs` | `[ ]` |

**Art S1 — Placeholder**

| # | Asset | Format | Outil | Statut |
|---|-------|--------|-------|--------|
| A1-1 | Barre de vie UI (rectangles colorés) | Unity UI (Canvas) | Unity Editor | `[ ]` |
| A1-2 | Hitbox slash visible (Gizmos en editor, invisible en play) | — | Unity Gizmos | `[ ]` |
| A1-3 | Flash blanc sur le player lors d'un dégât (iframes feedback) | Material swap | Unity Shader Graph / Sprite | `[ ]` |

---

### Sprint 2 — Ennemi de base

**Objectif :** il y a quelque chose à combattre.

**Tech**

| # | User Story | Acceptance Criteria | Fichiers | Statut |
|---|------------|---------------------|----------|--------|
| S2-1 | En tant que designer, je configure un ennemi via SO | Toutes les stats éditables dans l'Inspector sans toucher au code | `EnemyData.cs` | `[ ]` |
| S2-2 | En tant que joueur, un ennemi patrouille et m'inflige des dégâts au contact | Demi-tour au bord de plateforme ou sur obstacle | `EnemyController.cs` | `[ ]` |
| S2-3 | En tant que joueur, l'ennemi meurt quand ses PV tombent à 0 | Animation de mort + disparition, `OnEnemyDied` levé | `EnemyController.cs`, `OnEnemyDied.asset` | `[ ]` |

**Art S2 — Placeholder**

| # | Asset | Format | Outil | Statut |
|---|-------|--------|-------|--------|
| A2-1 | Sprite ennemi placeholder (rectangle rouge) | PNG 32×32 | Piskel / LibreSprite | `[ ]` |
| A2-2 | Indication de direction de patrouille (Gizmos) | — | Unity Gizmos | `[ ]` |

---

### Sprint 3 — Checkpoint & Save

**Objectif :** la mort n'est pas une fin — la boucle est fermée.

**Tech**

| # | User Story | Acceptance Criteria | Fichiers | Statut |
|---|------------|---------------------|----------|--------|
| S3-1 | En tant que joueur, j'active un checkpoint en m'y approchant | Feedback visuel à l'activation, `OnCheckpointActivated` levé | `CheckpointController.cs`, `SaveData.cs`, `OnCheckpointActivated.asset` | `[ ]` |
| S3-2 | En tant que joueur, je réapparais au dernier checkpoint activé après ma mort | Position correcte, scène correcte | `SaveManager.cs` | `[ ]` |
| S3-3 | En tant que joueur, mes PV sont restaurés au respawn | PV pleins à la réapparition | `SaveManager.cs` → `HealthController` | `[ ]` |

**Art S3 — Placeholder**

| # | Asset | Format | Outil | Statut |
|---|-------|--------|-------|--------|
| A3-1 | Sprite checkpoint inactif (forme géométrique grise) | PNG 32×64 | Piskel / LibreSprite | `[ ]` |
| A3-2 | Sprite checkpoint actif (même forme, colorée) | PNG 32×64 | Piskel / LibreSprite | `[ ]` |

---

### Sprint 4 — Transition de salles

**Objectif :** le monde est interconnecté.

**Tech**

| # | User Story | Acceptance Criteria | Fichiers | Statut |
|---|------------|---------------------|----------|--------|
| S4-1 | En tant que joueur, je passe d'une scène à l'autre via un trigger de porte | Chargement de la scène cible sans erreur | `SceneTransitionController.cs`, `TransitionData.cs` | `[ ]` |
| S4-2 | En tant que joueur, je réapparais au bon point d'entrée dans la nouvelle scène | Spawn point correspondant à la porte empruntée | `SceneTransitionController.cs` | `[ ]` |
| S4-3 | Le jeu contient 2 salles jouables connectées | Aller-retour possible entre les deux salles | Assets de niveau | `[ ]` |

**Art S4 — Level Design placeholder**

| # | Asset | Format | Outil | Statut |
|---|-------|--------|-------|--------|
| A4-1 | Salle 1 — layout plateforme (blockout gris) | Tilemap Unity | LDtk + import Unity | `[ ]` |
| A4-2 | Salle 2 — layout plateforme (blockout gris) | Tilemap Unity | LDtk + import Unity | `[ ]` |
| A4-3 | Trigger porte visible (rectangle coloré) | Unity Sprite | Unity Editor | `[ ]` |

---

### Sprint 5 — Capacité débloquable

**Objectif :** le joueur progresse — c'est un Metroidvania.

**Tech**

| # | User Story | Acceptance Criteria | Fichiers | Statut |
|---|------------|---------------------|----------|--------|
| S5-1 | En tant que designer, je définis une capacité via SO | SO configurable (nom, icône, input) sans modifier le code | `AbilityData.cs` | `[ ]` |
| S5-2 | En tant que joueur, je ramasse un item qui débloque le double saut | Capacité persistante après respawn | `AbilityUnlockController.cs`, `OnAbilityUnlocked.asset` | `[ ]` |
| S5-3 | Le double saut permet d'atteindre une zone inaccessible auparavant | Zone de validation dans le level design | Level design | `[ ]` |

**Art S5 — Placeholder**

| # | Asset | Format | Outil | Statut |
|---|-------|--------|-------|--------|
| A5-1 | Sprite item double saut (orbe colorée) | PNG 16×16 | Piskel / LibreSprite | `[ ]` |
| A5-2 | Icône double saut dans l'UI (petit sprite) | PNG 16×16 | Piskel / LibreSprite | `[ ]` |

---

## 4. Outils recommandés (solo dev, gratuits)

| Besoin | Outil | Lien |
|--------|-------|------|
| Pixel art & animation sprites | **LibreSprite** (fork Aseprite gratuit) | libresprite.github.io |
| Pixel art simple / rapide | **Piskel** (web, sans install) | piskelapp.com |
| Dessin / concept art | **Krita** | krita.org |
| Level design 2D | **LDtk** (s'intègre très bien avec Unity) | ldtk.io |
| Polices pixel art | **DaFont** catégorie Pixel | dafont.com |
| Sprites / tiles gratuits (prototypage) | **Kenney Assets** | kenney.nl |

> **Workflow recommandé :** LibreSprite pour les sprites, LDtk pour le level design, Kenney Assets pour les placeholders si tu veux aller vite.

---

## 5. Architecture technique (rappel)

```
Donnée configurable  →  ScriptableObject (.cs + .asset dans Assets/Data/)
Communication        →  GameEvent SO + GameEventListener
Comportement         →  MonoBehaviour (Controller)
```

Convention de nommage complète → voir [CLAUDE.md](../CLAUDE.md)

---

---

## 6. Critères de succès MVP

- [ ] Le joueur peut compléter la boucle : explorer → combattre → mourir → respawn → progresser
- [ ] Deux salles connectées et navigables
- [ ] Au moins un ennemi fonctionnel avec IA basique
- [ ] Checkpoint sauvegarde la progression
- [ ] Double saut débloquable ouvre une zone neue
