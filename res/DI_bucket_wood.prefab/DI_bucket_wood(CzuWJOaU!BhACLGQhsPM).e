13
12090332938241
12350549976724 1747406214146737100
{
  "name": "DI_bucket_wood",
  "local_enabled": true,
  "local_position": {
    "X": 39.9242248535156250,
    "Y": -2.8714294433593750
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
  "aoid": "12350550461716:1747406214146871300",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "bucket_wood",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "12350550527164:1747406214146889700",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "bucket_wood"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "12350550608812:1747406214146912000",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.1485595703125000,
        "Y": -0.0569000244140625
      },
      {
        "X": 0.1515655517578125,
        "Y": -0.0576629638671875
      },
      {
        "X": 0.1645202636718750,
        "Y": 0.0922088623046875
      },
      {
        "X": -0.1711730957031250,
        "Y": 0.1024093627929688
      }
    ]
  }
}
