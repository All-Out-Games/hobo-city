13
1657857376257
1858289179703520 1754332941484647300
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": -39.2535324096679688,
    "Y": 13.4912509918212891
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858271010509517:1754332937251270500",
  "next_sibling": "1858297548646827:1754332943434590100",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858289180058313:1754332941484729300",
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
  "aoid": "1858289180183443:1754332941484758400",
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
  "aoid": "1858289180230442:1754332941484769400",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858289180058313:1754332941484729300",
    "Skeleton": "1858289180250652:1754332941484774100"
  }
}
