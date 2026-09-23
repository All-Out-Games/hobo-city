14
1395864371207
2521613973
{
  "name": "DI_chair_diner_back",
  "local_enabled": true,
  "local_position": {
    "X": -25.9022369384765625,
    "Y": -54.8890991210937500
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 156405781,
  "next_sibling": 2156880796,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_chair_diner_back.prefab"
},
{
  "cid": 1,
  "aoid": 3459096630,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "chair_diner_back",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": 848427752,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "chair_diner_back"
    ],
    "depth_offset": 0.5847324132919312
  }
},
{
  "cid": 3,
  "aoid": 9379860,
  "component_type": "Mono_Component",
  "mono_component_type": "Chair",
  "data": {
    "Offset": {
      "X": 0.0500000007450581,
      "Y": 0.4000000059604645
    },
    "FaceRight": true
  }
},
{
  "cid": 4,
  "aoid": 3658007736,
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
