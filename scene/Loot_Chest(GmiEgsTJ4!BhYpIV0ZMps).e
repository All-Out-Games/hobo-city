13
1709396983809
1858316933345912 1754332947951176300
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": -28.5199966430664062,
    "Y": 21.0174350738525391
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858311425618930:1754332946667890500",
  "next_sibling": "1861620146484105:1754333717591551300",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858316933650309:1754332947951246500",
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
  "aoid": "1858316933771999:1754332947951274900",
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
  "aoid": "1858316933813064:1754332947951284400",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858316933650309:1754332947951246500",
    "Skeleton": "1858316933832715:1754332947951289000"
  }
}
