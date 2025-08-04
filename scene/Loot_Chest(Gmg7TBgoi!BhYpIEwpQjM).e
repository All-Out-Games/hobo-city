13
1541893259265
1858238320871970 1754332929634666700
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 17.4521331787109375,
    "Y": -30.5280647277832031
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858220295347473:1754332925434765200",
  "next_sibling": "1858248049453364:1754332931901402300",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858238321119435:1754332929634724000",
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
  "aoid": "1858238321230074:1754332929634749800",
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
  "aoid": "1858238321268731:1754332929634758800",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858238321119435:1754332929634724000",
    "Skeleton": "1858238321289672:1754332929634763700"
  }
}
