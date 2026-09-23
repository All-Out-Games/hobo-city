14
6356551598081
1837036449
{
  "name": "DI_FenceCopper_horizontal",
  "local_enabled": true,
  "local_position": {
    "X": 45.0164031982421875,
    "Y": -39.6222076416015625
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 1607470473,
  "next_sibling": 1454273352,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FenceCopper_horizontal.prefab"
},
{
  "cid": 1,
  "aoid": 2644113383,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "fence_copper_horizontal",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": 3697163096,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "fence_copper_horizontal"
    ],
    "depth_offset": 0.1593647003173828
  }
},
{
  "cid": 3,
  "aoid": 3595076742,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.2201080322265625,
        "Y": 0.4427566528320312
      },
      {
        "X": -1.2296447753906250,
        "Y": 0.0047168731689453
      },
      {
        "X": 1.2324829101562500,
        "Y": 0.0003528594970703
      },
      {
        "X": 1.2345733642578125,
        "Y": 0.4438877105712891
      }
    ]
  }
}
