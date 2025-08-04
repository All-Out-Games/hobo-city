13
16432544874497
34905681195878 1747848052061462100
{
  "name": "DI_car_long",
  "local_enabled": true,
  "local_position": {
    "X": 36.6427536010742188,
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
  "aoid": "34905681773966:1747848052061622000",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_long",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "34905681835598:1747848052061639100",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_long"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "34905681908462:1747848052061659300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -2.0536499023437500,
        "Y": -0.1752929687500000
      },
      {
        "X": 2.0194549560546875,
        "Y": -0.1927413940429688
      },
      {
        "X": 2.0630798339843750,
        "Y": 0.7415924072265625
      },
      {
        "X": -2.0765075683593750,
        "Y": 0.7517166137695312
      }
    ]
  }
}
