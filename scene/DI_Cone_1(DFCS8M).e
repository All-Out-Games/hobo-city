14
2190433320986
3305713420
{
  "name": "DI_Cone_1",
  "local_enabled": true,
  "local_position": {
    "X": 15.4855651855468750,
    "Y": -93.2887039184570312
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 3934228588,
  "next_sibling": 1565875050,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Cone_1.prefab"
},
{
  "cid": 1,
  "aoid": 1589638579,
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
  "aoid": 1871133942,
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
  "aoid": 2855715937,
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
