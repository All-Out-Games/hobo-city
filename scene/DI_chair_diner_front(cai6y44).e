14
3122441224201
2156880796
{
  "name": "DI_chair_diner_front",
  "local_enabled": true,
  "local_position": {
    "X": -25.9994659423828125,
    "Y": -55.7616882324218750
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 2521613973,
  "next_sibling": 1775078498,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_chair_diner_front.prefab"
},
{
  "cid": 1,
  "aoid": 2027602295,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "chair_diner_front",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": 3632097487,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "chair_diner_front"
    ],
    "depth_offset": 0.6196220517158508
  }
},
{
  "cid": 3,
  "aoid": 1236455965,
  "component_type": "Mono_Component",
  "mono_component_type": "Chair",
  "data": {
    "Offset": {
      "X": 0.1000000014901161,
      "Y": 0.4000000059604645
    },
    "FaceRight": true
  }
},
{
  "cid": 4,
  "aoid": 2748717749,
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
