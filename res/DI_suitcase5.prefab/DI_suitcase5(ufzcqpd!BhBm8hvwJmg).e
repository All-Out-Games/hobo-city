13
16544214024193
3195245079133 1747849415567120800
{
  "name": "DI_suitcase5",
  "local_enabled": true,
  "local_position": {
    "X": 42.7497253417968750,
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
  "aoid": "3195245598397:1747849415567264400",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "suitcase5",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "3195245656897:1747849415567280700",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "suitcase5"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "3195245724145:1747849415567299400",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.1330261230468750,
        "Y": 0.2328567504882812
      },
      {
        "X": -0.1296081542968750,
        "Y": -0.0213775634765625
      },
      {
        "X": 0.1399307250976562,
        "Y": -0.0215835571289062
      },
      {
        "X": 0.1361999511718750,
        "Y": 0.2294311523437500
      }
    ]
  }
}
