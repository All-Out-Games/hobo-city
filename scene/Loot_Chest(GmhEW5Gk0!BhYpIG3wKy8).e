13
1559073128449
1858248049453364 1754332931901402300
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 23.3977584838867188,
    "Y": -9.2648906707763672
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858238320871970:1754332929634666700",
  "next_sibling": "1858258564300059:1754332934351334900",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858248049806652:1754332931901483800",
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
  "aoid": "1858248049929847:1754332931901512500",
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
  "aoid": "1858248049971600:1754332931901522300",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858248049806652:1754332931901483800",
    "Skeleton": "1858248049992584:1754332931901527100"
  }
}
