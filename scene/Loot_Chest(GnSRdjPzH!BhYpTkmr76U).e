13
1799591297025
1861629374168263 1754333719741578900
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
  "previous_sibling": "1861627513462074:1754333719308038700",
  "next_sibling": "1861637630843279:1754333721665363200",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1861629374533075:1754333719741663300",
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
  "aoid": "1861629374665730:1754333719741694200",
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
  "aoid": "1861629374706064:1754333719741703600",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1861629374533075:1754333719741663300",
    "Skeleton": "1861629374727435:1754333719741708600"
  }
}
