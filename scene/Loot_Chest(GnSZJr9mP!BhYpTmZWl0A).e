13
1825361100801
1861637630843279 1754333721665363200
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 20.8768768310546875,
    "Y": -8.7665748596191406
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1861629374168263:1754333719741578900",
  "next_sibling": "1861640133613663:1754333722248502600",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1861637631092120:1754333721665420900",
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
  "aoid": "1861637631203576:1754333721665446800",
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
  "aoid": "1861637631245587:1754333721665456600",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1861637631092120:1754333721665420900",
    "Skeleton": "1861637631267990:1754333721665461800"
  }
}
