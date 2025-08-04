13
1773821493249
1861625367281481 1754333718807984300
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 22.3425064086914062,
    "Y": -6.0405049324035645
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1861623365695746:1754333718341620000",
  "next_sibling": "1861627513462074:1754333719308038700",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1861625367559304:1754333718808048500",
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
  "aoid": "1861625367679145:1754333718808076400",
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
  "aoid": "1861625367716770:1754333718808085100",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1861625367559304:1754333718808048500",
    "Skeleton": "1861625367736464:1754333718808089700"
  }
}
