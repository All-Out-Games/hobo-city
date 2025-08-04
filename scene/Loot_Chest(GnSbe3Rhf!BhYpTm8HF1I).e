13
1838246002689
1861640133613663 1754333722248502600
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 20.1733779907226562,
    "Y": -12.3133945465087891
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1861637630843279:1754333721665363200",
  "next_sibling": "1861641993221138:1754333722681786500",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1861640133985011:1754333722248588500",
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
  "aoid": "1861640134104895:1754333722248616800",
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
  "aoid": "1861640134149615:1754333722248626900",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1861640133985011:1754333722248588500",
    "Skeleton": "1861640134170513:1754333722248631800"
  }
}
