13
16647293239298
6709447874026 1747850391725299400
{
  "name": "DI_claw_r",
  "local_enabled": true,
  "local_position": {
    "X": 61.4700393676757812,
    "Y": 7.6976852416992188
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
  "aoid": "6709448399158:1747850391725444800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "claw_r",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "6709448458054:1747850391725461200",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "claw_r"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "6709448530594:1747850391725481300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": 0.0887985229492188,
        "Y": 0.2713546752929688
      },
      {
        "X": -0.1220169067382812,
        "Y": 0.2506637573242188
      },
      {
        "X": -0.2088317871093750,
        "Y": 0.0630035400390625
      },
      {
        "X": -0.1240005493164062,
        "Y": -0.0949630737304688
      },
      {
        "X": 0.0781745910644531,
        "Y": -0.1429214477539062
      },
      {
        "X": 0.2492904663085938,
        "Y": -0.0649261474609375
      },
      {
        "X": 0.2991600036621094,
        "Y": 0.0871505737304688
      },
      {
        "X": 0.2541351318359375,
        "Y": 0.2408142089843750
      }
    ]
  }
}
