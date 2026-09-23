14
3161095929863
2915484701
{
  "name": "DI_Cone_1",
  "local_enabled": true,
  "local_position": {
    "X": 0.1699600219726562,
    "Y": -46.4704971313476562
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 2699770472,
  "next_sibling": 1538739296,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Cone_1.prefab"
},
{
  "cid": 1,
  "aoid": 3390049907,
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
  "aoid": 655642024,
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
  "aoid": 1966180623,
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
