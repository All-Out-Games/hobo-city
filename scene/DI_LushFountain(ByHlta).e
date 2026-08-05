14
6339371728899
1914592090
{
  "name": "DI_LushFountain",
  "local_enabled": true,
  "local_position": {
    "X": 46.1982879638671875,
    "Y": -39.0420074462890625
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 580136334,
  "next_sibling": 1607470473,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_LushFountain.prefab"
},
{
  "cid": 1,
  "aoid": 3658822784,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "lush_fountain",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": 3013359546,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "lush_fountain"
    ],
    "depth_offset": 0.8560333251953125
  }
},
{
  "cid": 3,
  "aoid": 4260657546,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -1.4421081542968750,
        "Y": 1.4180355072021484
      },
      {
        "X": -1.4514770507812500,
        "Y": 0.0145511627197266
      },
      {
        "X": 1.4329833984375000,
        "Y": 0.0141716003417969
      },
      {
        "X": 1.4490509033203125,
        "Y": 1.4229488372802734
      }
    ]
  }
}
