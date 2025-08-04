13
16501264351233
2495826004187 1747849221285888500
{
  "name": "DI_flight_board",
  "local_enabled": true,
  "local_position": {
    "X": 53.1547660827636719,
    "Y": 2.1018218994140625
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
  "aoid": "2495826558839:1747849221286041700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "flight_board",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "2495826623063:1747849221286059600",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "flight_board"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "2495826699131:1747849221286080700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.6714973449707031,
        "Y": 0.1744689941406250
      },
      {
        "X": -0.6839942932128906,
        "Y": 0.0178070068359375
      },
      {
        "X": 0.6743240356445312,
        "Y": 0.0191726684570312
      },
      {
        "X": 0.6852340698242188,
        "Y": 0.1626815795898438
      }
    ]
  }
}
