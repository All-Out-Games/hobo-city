13
1722281885697
1861620146484105 1754333717591551300
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 22.8994445800781250,
    "Y": -11.4340200424194336
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858316933345912:1754332947951176300",
  "next_sibling": "1861623365695746:1754333718341620000",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1861620146744857:1754333717591611800",
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
  "aoid": "1861620146875749:1754333717591642300",
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
  "aoid": "1861620146919609:1754333717591652500",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1861620146744857:1754333717591611800",
    "Skeleton": "1861620146945753:1754333717591658500"
  }
}
