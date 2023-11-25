# DarkArise

## 20/11 - 25/11 Update (Final)
### V 0.0.8

#### Added

Options menu.

Sounds and music.

Music manager.

New cave map.

Transition from camp to cave maps.

Health and dmg system on enemies.

Attack system on player commands.


#### Ongoing tasks

Enemie attack system.

UI scenes.

Boss map.

#### Bugs found

Second character doesn't disapear after performing attack.

Enemies didn't disapear on death. Solved by disabling all of their components on death.

---


## 14/11 - 20/11 Update (Final)
### V 0.0.7

#### Added

Main menu.

Animation on main menu.

Escape menu.

Enemy.

#### Ongoing tasks

UI scenes.

Merchants.

Boss map.

Cutting back boss' sprites for animations to run propperly.

#### Bugs found

Second character doesn't disapear after performing attack.

---

## 7/11 - 13/11 Update
### V 0.0.6

#### Added

Second character attack on command.

#### Ongoing tasks

Main menu, settings and UI scenes.

Enemies.

Merchants.

Boss map.

Cutting back boss' sprites for animations to run propperly. 

#### Bugs found

Second character doesn't disapear after performing attack.

Player's animations doesn't run propperly on some circumstances.

---

## 31/10 - 06/11 Update
### V 0.0.5

#### Added

New character movement and attacks script.

Change between playable characters.

New map

#### Ongoing tasks

Script to change scenes.

More maps

AI for enemies.

#### Problems found

Saving project undos all the animator states and triggers of new character.

Change between playable characters not working well (Bind on key not working, looking for docs).

### Problems solved

Animator states and triggers need to be in a controller apart so it can be saved properly (hotfixed).

---

## 24/10 - 30/10 Update
### V 0.0.4

#### Added

Following camera.

New playable character.

New enemy.

#### Ongoing tasks

New maps.

AI for enemies.

Change between playable characters.

Bind new character's animation to keyboard controls.


#### Problems found

New character animations are not well framed (Fixing)

---

## 19/10 Update
### V 0.0.3

#### Added

Parallax done

#### Ongoing tasks

Making maps to test transitions.

---

## 16/10 Update
### V 0.0.2

#### Added

Added background tiles.

Prepared background for parallax code (separated in different layers).

Bugfixed player animations gaps.

#### Problems found

TileMap not working properly with background tileset.

Not showing anything in scene at restart.

#### Problems fixed by

Cut and rescale every tile from background sheets.

Placed by hand on different layers to facilitate the following parallax.

Save scene and changed .gitignore file.

#### Ongoing tasks

Parallax code.

---

## 10/02 Update
### V 0.0.1

#### Added

Committed project with basic player movement, triggers and animations such as:
- Left/Right movement.
- Jump action.
- Land trigger for land animation.
- Attacks bounded to J,K,L and SHIFT keys.
- Left/Right, jump/fall, land and attacks animations linked to such actions.

#### Ongoing tasks

- Background sprites implementation.
- Bugfix player animation gaps.