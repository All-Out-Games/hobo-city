13
16466904612865
34960687718798 1747848067340898000
{
  "name": "DI_luggage_truck",
  "local_enabled": true,
  "local_position": {
    "X": 50.6978378295898438,
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
  "aoid": "34960688240618:1747848067341042300",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "luggage_truck",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "34960688304914:1747848067341060100",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "luggage_truck"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "34960688372954:1747848067341079000",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.8870620727539062,
        "Y": 0.9144287109375000
      },
      {
        "X": 1.0811614990234375,
        "Y": -0.9710922241210938
      },
      {
        "X": 2.1191101074218750,
        "Y": -0.0365829467773438
      },
      {
        "X": -0.4408187866210938,
        "Y": 1.8382949829101562
      }
    ]
  }
}
