13
1791001362433
1861627513462074 1754333719308038700
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 24.1598892211914062,
    "Y": -5.8646302223205566
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1861625367281481:1754333718807984300",
  "next_sibling": "1861629374168263:1754333719741578900",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1861627513716333:1754333719308097700",
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
  "aoid": "1861627513831014:1754333719308124400",
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
  "aoid": "1861627513872982:1754333719308134100",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1861627513716333:1754333719308097700",
    "Skeleton": "1861627513894568:1754333719308139200"
  }
}
