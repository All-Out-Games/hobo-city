13
6502580486148
852087918542135 1747353906878649000
{
  "name": "DI_TrafficLight",
  "local_enabled": true,
  "local_position": {
    "X": 8.9806289672851562,
    "Y": -72.5307312011718750
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "852717352164615:1747354081256319600",
  "next_sibling": "852087920256665:1747353906879123700",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_TrafficLight.prefab"
},
{
  "cid": 1,
  "aoid": "852087918905729:1747353906878749200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 190,
    "skin": "traffic_light",
    "RespawnTime": 30,
    "CashReward": 14
  }
},
{
  "cid": 2,
  "aoid": "852087918946075:1747353906878760400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "traffic_light"
    ],
    "depth_offset": 0.0670814514160156
  }
},
{
  "cid": 3,
  "aoid": "852087918986955:1747353906878771700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.2743377685546875,
        "Y": 0.0818138122558594
      },
      {
        "X": 0.0254211425781250,
        "Y": -0.0914268493652344
      },
      {
        "X": 0.3099899291992188,
        "Y": 0.0725975036621094
      },
      {
        "X": 0.0220184326171875,
        "Y": 0.2526741027832031
      }
    ]
  }
}
