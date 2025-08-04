13
1756641624065
1861623365695746 1754333718341620000
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 24.9220123291015625,
    "Y": -11.8737087249755859
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1861620146484105:1754333717591551300",
  "next_sibling": "1861625367281481:1754333718807984300",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1861623366109664:1754333718341715800",
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
  "aoid": "1861623366290866:1754333718341757900",
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
  "aoid": "1861623366347110:1754333718341771100",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1861623366109664:1754333718341715800",
    "Skeleton": "1861623366381037:1754333718341778900"
  }
}
