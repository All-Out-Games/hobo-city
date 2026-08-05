14
6382321401857
1820956052
{
  "name": "DI_FenceCopper_vertical",
  "local_enabled": true,
  "local_position": {
    "X": 54.3227920532226562,
    "Y": -39.3956298828125000
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 1454273352,
  "next_sibling": 2064744699,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FenceCopper_vertical.prefab"
},
{
  "cid": 1,
  "aoid": 822607309,
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
  "aoid": 2239000958,
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
  "aoid": 3314560478,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
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
