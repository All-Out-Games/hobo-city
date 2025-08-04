13
1580547964929
1858263714539876 1754332935551328100
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 6.2662887573242188,
    "Y": -5.9393711090087891
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858258564300059:1754332934351334900",
  "next_sibling": "1858271010509517:1754332937251270500",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858263714844316:1754332935551398300",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {
    "text": "Loot!",
    "hold_text": "Looting...",
    "required_hold_time": 1
  }
},
{
  "cid": 2,
  "aoid": "1858263714960846:1754332935551425500",
  "component_type": "Internal_Component",
  "internal_component_type": "Box_Collider",
  "data": {
    "is_trigger": true,
    "size": {
      "X": 0.8804168701171875,
      "Y": 0.8323416709899902
    },
    "offset": {
      "X": -0.0169525146484375,
      "Y": 0.2706859111785889
    }
  }
},
{
  "cid": 3,
  "aoid": "1858263715000363:1754332935551434600",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858263714844316:1754332935551398300",
    "Skeleton": "1858263715020487:1754332935551439300"
  }
}
