14
6425271074819
3982839693
{
  "name": "DI_PondExotic",
  "local_enabled": true,
  "local_position": {
    "X": 22.3290786743164062,
    "Y": -41.3398742675781250
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 398695927,
  "next_sibling": 798964871,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_PondExotic.prefab"
},
{
  "cid": 1,
  "aoid": 3130760734,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "pond_exotic",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": 3371437605,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "pond_exotic"
    ],
    "depth_offset": 1.0324993133544922
  }
},
{
  "cid": 3,
  "aoid": 1671122218,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -1.1252593994140625,
        "Y": 0.0188884735107422
      },
      {
        "X": 1.1186828613281250,
        "Y": 0.0228614807128906
      },
      {
        "X": 1.1078796386718750,
        "Y": 1.6123008728027344
      },
      {
        "X": -1.1275939941406250,
        "Y": 1.6152229309082031
      }
    ]
  }
}
