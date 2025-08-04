13
1683627180033
1858311425618930 1754332946667890500
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": -30.4571151733398438,
    "Y": 29.4327869415283203
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858303557427631:1754332944834621300",
  "next_sibling": "1858316933345912:1754332947951176300",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858311426007435:1754332946667979600",
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
  "aoid": "1858311426134801:1754332946668009400",
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
  "aoid": "1858311426176425:1754332946668019000",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858311426007435:1754332946667979600",
    "Skeleton": "1858311426196721:1754332946668023700"
  }
}
