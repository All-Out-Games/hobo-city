13
11952893984769
2736262885428 1747403543538309100
{
  "name": "DI_tire",
  "local_enabled": true,
  "local_position": {
    "X": 47.4197692871093750,
    "Y": 1.9445266723632812
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
  "aoid": "2736263423187:1747403543538458200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "tire",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "2736263499867:1747403543538479200",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "tire"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "2736263582559:1747403543538502200",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "points": [
      {
        "X": -0.3788833618164062,
        "Y": 0.1556930541992188
      },
      {
        "X": -0.2370147705078125,
        "Y": -0.0267486572265625
      },
      {
        "X": 0.0289001464843750,
        "Y": -0.0491561889648438
      },
      {
        "X": 0.2683639526367188,
        "Y": 0.0136871337890625
      },
      {
        "X": 0.4056396484375000,
        "Y": 0.1890563964843750
      },
      {
        "X": 0.4188079833984375,
        "Y": 0.3535537719726562
      },
      {
        "X": -0.4220809936523438,
        "Y": 0.3265991210937500
      }
    ]
  }
}
