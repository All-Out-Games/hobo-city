13
12055973199873
10608817008806 1747405730336892500
{
  "name": "DI_embroiderer",
  "local_enabled": true,
  "local_position": {
    "X": 36.4752426147460938,
    "Y": -2.9277648925781250
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
  "aoid": "10608817465754:1747405730337019000",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "embroiderer",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "10608817539662:1747405730337039400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "embroiderer"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "10608817621670:1747405730337062100",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.5227661132812500,
        "Y": -0.0138168334960938
      },
      {
        "X": 0.5360641479492188,
        "Y": -0.0088272094726562
      },
      {
        "X": 0.5227890014648438,
        "Y": 0.4734954833984375
      },
      {
        "X": -0.5233383178710938,
        "Y": 0.4585494995117188
      }
    ]
  }
}
