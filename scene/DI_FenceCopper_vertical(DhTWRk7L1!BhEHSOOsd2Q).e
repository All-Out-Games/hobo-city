13
5476083302401
990890052268789 1748554593881677200
{
  "name": "DI_FenceCopper_vertical",
  "local_enabled": true,
  "local_position": {
    "X": -3.3141403198242188,
    "Y": -35.9191703796386719
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "990890052058079:1748554593881622100",
  "next_sibling": "2365315591419663:1748574935907894400",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FenceCopper_vertical.prefab"
},
{
  "cid": 1,
  "aoid": "990890052669119:1748554593881782700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "fence_copper_vertical",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "990890052689107:1748554593881788000",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "fence_copper_vertical"
    ],
    "depth_offset": 1.7044639587402344
  }
},
{
  "cid": 3,
  "aoid": "990890052710919:1748554593881793800",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.0965423583984375,
        "Y": 1.6075839996337891
      },
      {
        "X": -0.1105194091796875,
        "Y": -0.0087661743164062
      },
      {
        "X": 0.0712585449218750,
        "Y": -0.0066986083984375
      },
      {
        "X": 0.0727233886718750,
        "Y": 1.6057415008544922
      }
    ]
  }
}
