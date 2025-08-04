13
12098922872833
13234238913232 1747406459613416000
{
  "name": "DI_car_tesla_white",
  "local_enabled": true,
  "local_position": {
    "X": 36.6942062377929688,
    "Y": 7.7483291625976562
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
  "aoid": "13234239424540:1747406459613557500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_tesla_white",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "13234239497368:1747406459613577500",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_tesla_white"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "13234239583120:1747406459613601300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.3304023742675781,
        "Y": -0.1302947998046875
      },
      {
        "X": 1.5457229614257812,
        "Y": -0.1265792846679688
      },
      {
        "X": 1.5103302001953125,
        "Y": 0.8051757812500000
      },
      {
        "X": -1.3149604797363281,
        "Y": 0.7954788208007812
      }
    ]
  }
}
