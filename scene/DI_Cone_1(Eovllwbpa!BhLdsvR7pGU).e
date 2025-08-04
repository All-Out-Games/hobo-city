13
2181843386394
1305091944594010 1750623497936081300
{
  "name": "DI_Cone_1",
  "local_enabled": true,
  "local_position": {
    "X": 23.4878234863281250,
    "Y": -90.5196228027343750
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "552227420576077:1745621596481246800",
  "next_sibling": "1305091945266708:1750623497936291700",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Cone_1.prefab"
},
{
  "cid": 1,
  "aoid": "1305091944734595:1750623497936124600",
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
  "aoid": "1305091944764200:1750623497936133900",
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
  "aoid": "1305091944789776:1750623497936141900",
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
