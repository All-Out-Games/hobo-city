13
1675037245441
1858303557427631 1754332944834621300
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": -39.8251419067382812,
    "Y": 27.6862049102783203
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "previous_sibling": "1858297548646827:1754332943434590100",
  "next_sibling": "1858311425618930:1754332946667890500",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "1858303557760021:1754332944834697700",
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
  "aoid": "1858303557886656:1754332944834727200",
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
  "aoid": "1858303557925227:1754332944834736200",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "1858303557760021:1754332944834697700",
    "Skeleton": "1858303557947286:1754332944834741400"
  }
}
