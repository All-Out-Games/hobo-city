13
12073153069057
12332790419004 1747406209213576200
{
  "name": "DI_printer",
  "local_enabled": true,
  "local_position": {
    "X": 37.9460296630859375,
    "Y": -2.9712448120117188
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
  "aoid": "12332790866772:1747406209213700100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "printer",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "12332791418508:1747406209213853300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "printer"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "12332791506096:1747406209213877600",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.4283065795898438,
        "Y": 0.0103149414062500
      },
      {
        "X": 0.4294967651367188,
        "Y": 0.0012969970703125
      },
      {
        "X": 0.4124145507812500,
        "Y": 0.5646286010742188
      },
      {
        "X": -0.4230957031250000,
        "Y": 0.5719985961914062
      }
    ]
  }
}
