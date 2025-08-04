13
11970073853953
3235010159700 1747403682077824600
{
  "name": "DI_chair_diner_front",
  "local_enabled": true,
  "local_position": {
    "X": 37.7882995605468750,
    "Y": -1.3473434448242188
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": "3235010605020:1747403682077947800",
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
  "aoid": "3235010670972:1747403682077965900",
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
  "aoid": "387030048084384:1748907168551900900",
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
  "aoid": "387038702929766:1748907170568456700",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
