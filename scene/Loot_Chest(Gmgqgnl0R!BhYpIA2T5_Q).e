13
1524713390081
1858220295347473 1754332925434765200
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 23.5102691650390625,
    "Y": -36.5293273925781250
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1572725884433228:1754266406018229300",
  "next_sibling": "1858238320871970:1754332929634666700",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858220295663609:1754332925434838000",
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
  "aoid": "1858220295778763:1754332925434864900",
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
  "aoid": "1858220295817893:1754332925434874100",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858220295663609:1754332925434838000",
    "Skeleton": "1858220295840812:1754332925434879300"
  }
}
