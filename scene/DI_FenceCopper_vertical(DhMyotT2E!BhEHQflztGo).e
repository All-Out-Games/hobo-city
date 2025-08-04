13
5450313498625
990439468776836 1748554475084108200
{
  "name": "DI_FenceCopper_vertical",
  "local_enabled": true,
  "local_position": {
    "X": 14.4749145507812500,
    "Y": -34.4014892578125000
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "952269338558135:1748286570073562500",
  "next_sibling": "990890052058079:1748554593881622100",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FenceCopper_vertical.prefab"
},
{
  "cid": 1,
  "aoid": "990439469056250:1748554475084181200",
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
  "aoid": "990439469098734:1748554475084192400",
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
  "aoid": "990439469151174:1748554475084206200",
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
