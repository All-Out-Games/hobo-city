14
7172595384323
2579075376
{
  "name": "DI_CarSedan_60s",
  "local_enabled": true,
  "local_position": {
    "X": 44.9673080444335938,
    "Y": -62.9957656860351562
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "previous_sibling": 835159602,
  "next_sibling": 838131239,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarSedan_60s.prefab"
},
{
  "cid": 1,
  "aoid": 1892689578,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "car_sedan_60s",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": 2662578899,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_sedan_60s"
    ],
    "depth_offset": 0.6421203613281250
  }
},
{
  "cid": 3,
  "aoid": 3264761303,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.8515625000000000,
        "Y": 0.0230770111083984
      },
      {
        "X": 1.1153564453125000,
        "Y": 0.0000057220458984
      },
      {
        "X": 1.4934234619140625,
        "Y": 0.2000637054443359
      },
      {
        "X": 1.5398559570312500,
        "Y": 1.1493911743164062
      },
      {
        "X": -1.4941406250000000,
        "Y": 1.1735382080078125
      },
      {
        "X": -1.4970855712890625,
        "Y": 0.1402492523193359
      }
    ]
  }
}
