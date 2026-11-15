# CS:CO Weapons

A CounterStrikeSharp plugin that puts the Counter-Strike: Classic Offensive weapon models on CS2 weapons.

The models and sounds come from the [CS:CO Weapons Workshop addon](https://steamcommunity.com/sharedfiles/filedetails/?id=3840407481). This plugin swaps the models in and adds a few CS:CO touches.

> Experimental. The weapons use the default CS2 animations for now.

## Features

- CS:CO models for 26 weapons, each one can be turned off in the config
- CS:CO weapon sounds (from the addon)
- CS 1.6 style zoom for the AUG and SG 553
- Optional VIP USP model for the USP-S, server-wide or per player with `!vipusp`

## Requirements

- [Metamod:Source 2.0](https://www.sourcemm.net/downloads.php?branch=dev)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)
- [MultiAddonManager](https://github.com/Source2ZE/MultiAddonManager)

## Installation

1. Download the latest release and extract it into `game/csgo/`, so the plugin ends up in
   `addons/counterstrikesharp/plugins/CscoWeapons/`.
2. Add the addon to MultiAddonManager in `cfg/multiaddonmanager/multiaddonmanager.cfg`:
   ```
   mm_extra_addons "3840407481"
   ```
3. Restart the server. Players download the addon automatically when they join.

## Configuration

The config is created on first start at
`addons/counterstrikesharp/configs/plugins/CscoWeapons/CscoWeapons.json`.

```json
{
  "Weapons": {
    "ak47": true,
    "m4a4": false
  },
  "VipUsp": "off",
  "ClassicZoomFov": 55,
  "ConfigVersion": 2
}
```

| Option | Description |
| --- | --- |
| `Weapons` | `true` uses the CS:CO model, `false` keeps the stock CS2 model. Names follow the CS2 weapon names. |
| `VipUsp` | `off`: normal CS:CO USP-S. `all`: everyone gets the VIP USP. `choice`: players switch with `!vipusp`. |
| `ClassicZoomFov` | FOV of the AUG / SG 553 zoom. Lower zooms in further. |

Changes apply after a map change or server restart.

### Weapon names

| Config name | CS2 weapon | CS:CO model |
| --- | --- | --- |
| `glock` | Glock-18 | Glock-18 |
| `usp_silencer` | USP-S | USP |
| `hkp2000` | P2000 | USP (no silencer) |
| `p250` | P250 | P228 |
| `fiveseven` | Five-SeveN | Five-SeveN |
| `deagle` | Desert Eagle | Desert Eagle |
| `elite` | Dual Berettas | Dual Elites |
| `mp9` | MP9 | TMP |
| `mac10` | MAC-10 | MAC-10 |
| `mp5sd` | MP5-SD | MP5 |
| `ump45` | UMP-45 | UMP45 |
| `p90` | P90 | P90 |
| `ak47` | AK-47 | AK-47 |
| `m4a4` | M4A4 | M4A1 (ACOG) |
| `m4a1_silencer` | M4A1-S | M4A1 |
| `galilar` | Galil AR | Galil |
| `famas` | FAMAS | FAMAS |
| `sg556` | SG 553 | SG 552 |
| `aug` | AUG | AUG |
| `awp` | AWP | AWP |
| `ssg08` | SSG 08 | Scout |
| `scar20` | SCAR-20 | SG550 |
| `g3sg1` | G3SG1 | G3SG1 |
| `m249` | M249 | M249 |
| `nova` | Nova | M3 |
| `xm1014` | XM1014 | XM1014 |

## Notes

- With the CS:CO model on, the AUG and SG 553 always use the classic zoom, because the stock CS2 scope doesn't
  line up with the CS:CO scopes. Turn the gun off in the config to get the stock zoom back.
- The weapon sounds come from the addon, so they're on whenever the addon is mounted.

## Credits

- Counter-Strike: Classic Offensive team and the original weapon model authors
