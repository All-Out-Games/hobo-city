13
6373731467265
581064002313397 1745628475215908400
{
  "name": "DI_FenceCopper_horizontal",
  "local_enabled": true,
  "local_position": {
    "X": 53.1297531127929688,
    "Y": -39.6222076416015625
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "580950329259805:1745628448100115500",
  "next_sibling": "581162734332433:1745628498767637300",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FenceCopper_horizontal.prefab"
},
{
  "cid": 1,
  "aoid": "581064002601223:1745628475215976700",
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
  "aoid": "581064002673463:1745628475215993900",
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
  "aoid": "581064002764309:1745628475216015500",
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
