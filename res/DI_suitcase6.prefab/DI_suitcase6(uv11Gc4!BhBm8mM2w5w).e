13
16552803958785
3212464908088 1747849420350361200
{
  "name": "DI_suitcase6",
  "local_enabled": true,
  "local_position": {
    "X": 43.3163833618164062,
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
  "aoid": "3212465434156:1747849420350506700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "suitcase6",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "3212465494276:1747849420350523400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "suitcase6"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "3212465559220:1747849420350541500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2682647705078125,
        "Y": 0.1390457153320312
      },
      {
        "X": 0.1234130859375000,
        "Y": -0.0475540161132812
      },
      {
        "X": 0.2926101684570312,
        "Y": 0.0547790527343750
      },
      {
        "X": -0.1059188842773438,
        "Y": 0.2425231933593750
      }
    ]
  }
}
