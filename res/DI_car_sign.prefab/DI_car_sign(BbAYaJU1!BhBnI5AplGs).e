13
16630113370113
6253881890101 1747850265180393900
{
  "name": "DI_car_sign",
  "local_enabled": true,
  "local_position": {
    "X": 58.4454956054687500,
    "Y": 7.6976852416992188
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
  "aoid": "6253882369621:1747850265180526500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_sign",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "6253882429417:1747850265180543100",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_sign"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "6253882507105:1747850265180564700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": 0.0776214599609375,
        "Y": 0.3500823974609375
      },
      {
        "X": -0.3152084350585938,
        "Y": 0.3099975585937500
      },
      {
        "X": -0.4209480285644531,
        "Y": 0.0520401000976562
      },
      {
        "X": -0.3236885070800781,
        "Y": -0.1669921875000000
      },
      {
        "X": 0.0147399902343750,
        "Y": -0.2444305419921875
      },
      {
        "X": 0.3607254028320312,
        "Y": -0.1633834838867188
      },
      {
        "X": 0.4086151123046875,
        "Y": 0.0839767456054688
      },
      {
        "X": 0.3432655334472656,
        "Y": 0.3155670166015625
      }
    ]
  }
}
