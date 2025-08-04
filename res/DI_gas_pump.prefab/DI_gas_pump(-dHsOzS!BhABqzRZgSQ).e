13
11995843657729
4360594582738 1747403994737018000
{
  "name": "DI_gas_pump",
  "local_enabled": true,
  "local_position": {
    "X": 39.7413711547851562,
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
  "aoid": "4360595039362:1747403994737144200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "gas_pump",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "4360595108878:1747403994737163400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "gas_pump"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "4360595193550:1747403994737186900",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.3735122680664062,
        "Y": -0.1106796264648438
      },
      {
        "X": 0.3726959228515625,
        "Y": -0.1015243530273438
      },
      {
        "X": 0.3724441528320312,
        "Y": 0.2807769775390625
      },
      {
        "X": -0.3743820190429688,
        "Y": 0.2841110229492188
      }
    ]
  }
}
