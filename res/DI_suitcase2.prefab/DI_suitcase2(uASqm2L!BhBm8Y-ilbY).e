13
16518444220417
3161409088907 1747849406168323800
{
  "name": "DI_suitcase2",
  "local_enabled": true,
  "local_position": {
    "X": 41.0554275512695312,
    "Y": -2.9518356323242188
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
  "aoid": "3161409645287:1747849406168477800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "suitcase2",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "3161409707603:1747849406168495100",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "suitcase2"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "3161409784607:1747849406168516500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2524566650390625,
        "Y": -0.0058135986328125
      },
      {
        "X": -0.0349884033203125,
        "Y": -0.1203002929687500
      },
      {
        "X": 0.2402648925781250,
        "Y": 0.0242385864257812
      },
      {
        "X": -0.0033950805664062,
        "Y": 0.1465301513671875
      }
    ]
  }
}
