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

---

## Conventions de nommage

| Type | Convention | Exemple |
|------|-----------|---------|
| SO données | `NomData` | `CharacterData`, `EnemyData` |
| SO events | `OnNomVerbe` | `OnPlayerDied`, `OnCoinCollected` |
| Controllers | `NomController` | `PlayerController`, `EnemyController` |
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

## État actuel

- `PlayerController.cs` — mouvement + saut (coyote time, jump buffer, variable fall gravity)
- `CharacterData.cs` — SO données du player (moveSpeed, jumpForce, etc.)
- `GameEvent.cs` / `GameEventListener.cs` — base du système d'events (à câbler selon besoins)
