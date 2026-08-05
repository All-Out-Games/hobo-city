14
2207613190170
2699770472
{
  "name": "DI_Cone_1",
  "local_enabled": true,
  "local_position": {
    "X": -0.9279937744140625,
    "Y": -46.4704971313476562
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 4048131997,
  "next_sibling": 2915484701,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Cone_1.prefab"
},
{
  "cid": 1,
  "aoid": 1412810642,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 10,
    "skin": "cone_1",
    "RespawnTime": 60,
    "CashReward": 3
  }
},
{
  "cid": 2,
  "aoid": 3728355733,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "cone_1"
    ],
    "depth_offset": 0.0854511260986328
  }
},
{
  "cid": 3,
  "aoid": 2952101066,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.3152923583984375,
        "Y": 0.1086959838867188
      },
      {
        "X": -0.0068359375000000,
        "Y": -0.0829353332519531
      },
      {
        "X": 0.3392333984375000,
        "Y": 0.1074066162109375
      },
      {
        "X": -0.0010528564453125,
        "Y": 0.3714885711669922
      }
    ]
  }
}
