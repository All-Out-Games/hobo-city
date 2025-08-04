13
1481763717121
400308853860792 1744283455157971700
{
  "name": "Loot_Chest",
  "local_enabled": true,
  "local_position": {
    "X": 37.3900451660156250,
    "Y": -16.4604740142822266
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 0.8000000119209290,
    "Y": 0.8000000119209290
  },
  "next_sibling": "1572716728334480:1754266403884881500",
  "parent": "1572508562982168:1754266355382885900",
  "spawn_as_networked_entity": true,
  "network_position": true,
  "linked_prefab": "Loot_Chest.prefab"
},
{
  "cid": 1,
  "aoid": "400363210395199:1744283469490139100",
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
  "aoid": "400376044678673:1744283472874150300",
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
  "aoid": "409744146170752:1744285942958359800",
  "component_type": "Mono_Component",
  "mono_component_type": "LootChest",
  "data": {
    "Interactable": "400363210395199:1744283469490139100",
    "Skeleton": "400419654584587:1744283484372759800"
  }
}
