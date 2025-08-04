13
11987253723137
4064295254500 1747403912432477300
{
  "name": "DI_chair_diner_back",
  "local_enabled": true,
  "local_position": {
    "X": 38.7653884887695312,
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
  "aoid": "4064295712636:1747403912432604300",
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
  "aoid": "4064295799864:1747403912432628000",
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
  "aoid": "387003430610610:1748907162350101500",
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
  "aoid": "387012507741594:1748907164465048500",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
