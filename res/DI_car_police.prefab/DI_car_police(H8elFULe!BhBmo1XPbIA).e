13
16441134809089
34942328586974 1747848062241190400
{
  "name": "DI_car_police",
  "local_enabled": true,
  "local_position": {
    "X": 40.5521697998046875,
    "Y": 10.2728805541992188
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
  "aoid": "34942329105086:1747848062241333800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_police",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "34942329174134:1747848062241352900",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_police"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "34942329245954:1747848062241372800",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.5482025146484375,
        "Y": -0.1744079589843750
      },
      {
        "X": 1.7296600341796875,
        "Y": -0.1956634521484375
      },
      {
        "X": 1.7232513427734375,
        "Y": 0.7249374389648438
      },
      {
        "X": -1.5716247558593750,
        "Y": 0.7233200073242188
      }
    ]
  }
}
