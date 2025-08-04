13
1490353651713
1572725884433228 1754266406018229300
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 39.4871215820312500,
    "Y": -42.5283508300781250
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1572716728334480:1754266403884881500",
  "next_sibling": "1858220295347473:1754332925434765200",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1572725884919429:1754266406018341700",
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
  "aoid": "1572725885039399:1754266406018369700",
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
  "aoid": "1572725885077841:1754266406018378600",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1572725884919429:1754266406018341700",
    "Skeleton": "1572725885098739:1754266406018383400"
  }
}
