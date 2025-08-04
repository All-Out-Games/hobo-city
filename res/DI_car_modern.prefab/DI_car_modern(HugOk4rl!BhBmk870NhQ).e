13
16423954939905
33982025796325 1747847795493099600
{
  "name": "DI_car_modern",
  "local_enabled": true,
  "local_position": {
    "X": 50.4697189331054688,
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
  "aoid": "33982026352165:1747847795493253400",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_modern",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "33982026413077:1747847795493270300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_modern"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "33982026477985:1747847795493288300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.1484832763671875,
        "Y": -0.0953063964843750
      },
      {
        "X": 1.3078308105468750,
        "Y": -0.0985870361328125
      },
      {
        "X": 1.3563995361328125,
        "Y": 0.7771835327148438
      },
      {
        "X": -1.1820144653320312,
        "Y": 0.7954788208007812
      }
    ]
  }
}
