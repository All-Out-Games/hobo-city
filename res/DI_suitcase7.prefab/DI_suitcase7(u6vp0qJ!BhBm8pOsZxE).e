13
16561393893377
3224172448393 1747849423602424900
{
  "name": "DI_suitcase7",
  "local_enabled": true,
  "local_position": {
    "X": 43.9952926635742188,
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
  "aoid": "3224173016617:1747849423602582100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "suitcase7",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "3224173076197:1747849423602598700",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "suitcase7"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "3224173148665:1747849423602618800",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2377319335937500,
        "Y": 0.0146865844726562
      },
      {
        "X": 0.2215728759765625,
        "Y": 0.0113525390625000
      },
      {
        "X": 0.2337265014648438,
        "Y": 0.1922302246093750
      },
      {
        "X": -0.2280654907226562,
        "Y": 0.1967086791992188
      }
    ]
  }
}
