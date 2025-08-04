13
12081743003649
12342234064870 1747406211836784900
{
  "name": "DI_fruit_crate",
  "local_enabled": true,
  "local_position": {
    "X": 39.0602874755859375,
    "Y": -2.8865737915039062
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
  "aoid": "12342234514834:1747406211836909300",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "fruit_crate",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "12342234581614:1747406211836927700",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "fruit_crate"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "12342234666178:1747406211836951200",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.3641052246093750,
        "Y": 0.0312042236328125
      },
      {
        "X": 0.1450576782226562,
        "Y": -0.1278762817382812
      },
      {
        "X": 0.3827972412109375,
        "Y": 0.0195846557617188
      },
      {
        "X": -0.0748748779296875,
        "Y": 0.1623611450195312
      }
    ]
  }
}
