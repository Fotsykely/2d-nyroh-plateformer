# Nyroh — Conventions du projet

Projet : **Metroidvania 2D** Unity. Architecture orientée **ScriptableObjects** pour découpler données et comportement.

---

## Vision du jeu

Nyroh est un **Metroidvania 2D** — références directes : *Hollow Knight / Silksong* et *9 Sols*.

Caractéristiques cibles :
- Monde interconnecté, exploration non-linéaire, backtracking avec nouvelles capacités
- Système de capacités débloquables (double saut, dash, wall jump, parry, etc.)
- Combat précis : parry/counter, combos, boss fights
- Checkpoints + save système persistant
- Map révélée progressivement
- Ennemis avec IA variée, patterns distincts

Systèmes à construire (ordre suggéré) :
1. Capacités joueur (`AbilityData` SO + `UnlockSystem`)
2. Santé / dégâts (`HealthData` SO + `DamageSystem`)
3. Combat de base (`AttackController` + parry)
4. Save / Checkpoint (`SaveData` SO + `SaveManager`)
5. Transition de salles (`SceneTransitionController`)
6. IA ennemis (`EnemyData` SO + `EnemyController`)
7. Boss (`BossData` SO + `BossController`)
8. Map (`MapData` SO + `MapController`)

---

## Architecture : deux patterns fondamentaux

### 1. ScriptableObject Data (SO)
Toute valeur configurable vit dans un SO, pas dans un MonoBehaviour.

```
CharacterData.cs   →  ScriptableObject  (données : stats, vitesse, forces)
PlayerController.cs →  MonoBehaviour    (comportement : lit les données, agit)
CharacterData.asset →  instance dans Assets/Data/
```

Règle : si c'est un nombre, une courbe, ou une liste dans l'Inspector → SO.

### 2. SO Event Channel
Communication entre systèmes sans référence directe.

```
GameEvent.cs           →  SO (canal d'événement, base commune)
GameEventListener.cs   →  MonoBehaviour (écoute un GameEvent, déclenche une UnityEvent)
OnPlayerDied.asset     →  instance dans Assets/Data/Events/
```

Flux : `PlayerController` appelle `OnPlayerDied.Raise()` → `GameEventListener` sur le GameManager déclenche `ReloadScene()`.

---

## Règle : créer un nouveau système

Chaque nouveau système (ennemi, item, capacité) suit toujours cette structure :

```
Assets/Scripts/Data/      → NomDuSystemeData.cs       (ScriptableObject)
Assets/Scripts/Events/    → OnNomDuSystemeEvent.cs     (GameEvent ou sous-classe)
Assets/Scripts/           → NomDuSystemeController.cs  (MonoBehaviour)
Assets/Prefabs/           → _NomDuSysteme.prefab
Assets/Data/              → NomDuSystemeData.asset
Assets/Data/Events/       → OnNomDuSystemeXxx.asset
```

**Répartition du travail** : Claude écrit uniquement les fichiers `.cs`. Les instances `.asset` (SO données/events), les prefabs, et tout câblage de composants/références (Add Component, drag & drop dans l'Inspector) sont créés à la main par l'utilisateur dans l'Unity Editor — jamais par édition directe de YAML. Claude explique quoi créer et où (nom, menu `Create >`, champs à assigner), pas plus.

---

## Conventions de nommage

| Type | Convention | Exemple |
|------|-----------|---------|
| SO données | `NomData` | `CharacterData`, `EnemyData` |
| SO events | `OnNomVerbe` | `OnPlayerDied`, `OnCoinCollected` |
| Controllers (pilotent) | `NomController` | `PlayerController`, `PatrolEnemyController`, `RespawnController` |
| Handlers (réagissent) | `NomHandler` | `KnockbackHandler`, `CheckpointHandler`, `KillZoneHandler` |
| Checkers (capteurs passifs) | `NomChecker` | `GroundChecker` |

**Controller vs Handler — test concret** : si le script a un `Update`/`FixedUpdate` qui pilote un comportement dans la durée, ou orchestre une séquence à plusieurs étapes (ex. routine async avec délai) → `Controller`. S'il se contente de réagir une seule fois à un trigger/event externe par une action immédiate et autonome (typiquement `OnTriggerEnter2D` → une seule ligne d'effet) → `Handler`. (`DamageDealer` est une exception historique qui précède cette règle — pas la renommer rétroactivement sans besoin.)
| Prefabs root | préfixe `_` | `_Player`, `_World`, `_Systems` |
| Assets SO données | même nom que la classe | `CharacterData.asset` |
| Assets SO events | même nom que la classe | `OnPlayerDied.asset` |

---

## Structure des dossiers

```
Assets/
├── Data/
│   ├── Events/         ← instances de GameEvent (.asset)
│   └── *.asset         ← instances de SO données
├── Prefabs/
│   ├── Charactere/     ← _Player.prefab
│   ├── System/         ← _Systems, CinemachineCamera, Main Camera
│   └── World/          ← _World.prefab
├── Scripts/
│   ├── Data/           ← classes ScriptableObject (CharacterData.cs, etc.)
│   ├── Events/         ← GameEvent.cs, GameEventListener.cs
│   └── *.cs            ← MonoBehaviours (PlayerController.cs, etc.)
└── Settings/           ← URP, Renderer2D, GlobalLight configs
```

---

## Préfab Player — structure hiérarchie

```
_Player (Empty)           ← Rigidbody2D, Collider2D, PlayerController
└── Sprite                ← SpriteRenderer, Animator
└── GroundCheck           ← Transform seul, point de détection du sol
```

---

## Packages actifs

- **Input System** (new) — `InputSystem_Actions.cs` auto-généré, ne pas éditer
- **Cinemachine** — CinemachineCamera.prefab suit le player
- **Universal Render Pipeline 2D** — settings dans Assets/Settings/

---

## Philosophie de travail

Ce projet n'est **pas du vibe coding**. L'objectif est d'apprendre et d'appliquer les pratiques des game devs professionnels :
- Comprendre **pourquoi** chaque choix d'architecture avant de coder
- Utiliser l'**Unity Editor** en premier (hiérarchie, Inspector, prefabs) — le code vient en renfort, pas à la place
- Chaque système est discuté et validé avant d'être implémenté
- L'IA accompagne et explique, elle ne génère pas du code à la aveugle

---

## État actuel

- `PlayerController.cs` — mouvement + saut (coyote time, jump buffer, variable fall gravity)
- `CharacterData.cs` — SO données du player (moveSpeed, jumpForce, etc.)
- `GameEvent.cs` / `GameEventListener.cs` — base du système d'events (à câbler selon besoins)
