13
11978663788545
3244469138092 1747403684705292400
{
  "name": "DI_brick_pile",
  "local_enabled": true,
  "local_position": {
    "X": 41.7413711547851562,
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
  "aoid": "3244469600134:1747403684705420200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "brick_pile",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "3244469688298:1747403684705444300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "brick_pile"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "3244469769406:1747403684705466800",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.5687255859375000,
        "Y": -0.2973480224609375
      },
      {
        "X": 1.1067962646484375,
        "Y": -0.2193984985351562
      },
      {
        "X": 1.5164566040039062,
        "Y": 0.2007827758789062
      },
      {
        "X": 0.1832809448242188,
        "Y": 0.5597610473632812
      },
      {
        "X": -1.5840454101562500,
        "Y": 0.2615966796875000
      }
    ]
  }
}
