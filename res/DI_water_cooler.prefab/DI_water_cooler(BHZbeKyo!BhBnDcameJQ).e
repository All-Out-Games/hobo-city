13
16587163697153
4906387287208 1747849890879890000
{
  "name": "DI_water_cooler",
  "local_enabled": true,
  "local_position": {
    "X": 55.2024574279785156,
    "Y": 4.6439285278320312
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
  "aoid": "4906387866952:1747849890880050300",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "water_cooler",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "4906387927036:1747849890880066900",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "water_cooler"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "4906387996912:1747849890880086400",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.3593864440917969,
        "Y": 0.1392517089843750
      },
      {
        "X": 0.0951118469238281,
        "Y": -0.0574569702148438
      },
      {
        "X": 0.3333091735839844,
        "Y": 0.1866989135742188
      },
      {
        "X": -0.1237640380859375,
        "Y": 0.3285446166992188
      }
    ]
  }
}
