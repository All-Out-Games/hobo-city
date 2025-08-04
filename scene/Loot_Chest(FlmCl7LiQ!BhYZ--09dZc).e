13
1387274436609
1572716728334480 1754266403884881500
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 26.8442077636718750,
    "Y": -18.8646049499511719
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "400308853860792:1744283455157971700",
  "next_sibling": "1572725884433228:1754266406018229300",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1572716728707462:1754266403884967900",
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
  "aoid": "1572716728856758:1754266403885002500",
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
  "aoid": "1572716728906982:1754266403885014200",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1572716728707462:1754266403884967900",
    "Skeleton": "1572716728931406:1754266403885019900"
  }
}
