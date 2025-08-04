13
16484084482049
2273347030566 1747849159486760200
{
  "name": "DI_cart2",
  "local_enabled": true,
  "local_position": {
    "X": 51.3030242919921875,
    "Y": 2.1346969604492188
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
  "aoid": "2273347595514:1747849159486916200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "cart2",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "2273347665030:1747849159486935500",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "cart2"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "2273347753158:1747849159486960300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2223739624023438,
        "Y": 0.3208847045898438
      },
      {
        "X": -0.1749954223632812,
        "Y": -0.0953292846679688
      },
      {
        "X": 0.2751083374023438,
        "Y": -0.1006164550781250
      },
      {
        "X": 0.2826919555664062,
        "Y": 0.3224029541015625
      }
    ]
  }
}
