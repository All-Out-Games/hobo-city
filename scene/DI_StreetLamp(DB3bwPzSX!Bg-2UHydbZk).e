13
6451040878594
852632348341399 1747354057706944100
{
  "name": "DI_StreetLamp",
  "local_enabled": true,
  "local_position": {
    "X": 0.0528945922851562,
    "Y": -60.5697479248046875
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "852632346751737:1747354057706504000",
  "next_sibling": "852632349965923:1747354057707394100",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_StreetLamp.prefab"
},
{
  "cid": 1,
  "aoid": "852632348666877:1747354057707034100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 160,
    "skin": "street_lamp",
    "RespawnTime": 30,
    "CashReward": 10
  }
},
{
  "cid": 2,
  "aoid": "852632348698189:1747354057707042800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "street_lamp"
    ],
    "depth_offset": 0.0385437011718750
  }
},
{
  "cid": 3,
  "aoid": "852632348733051:1747354057707052500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.2653045654296875,
        "Y": -0.0610275268554688
      },
      {
        "X": 0.0206146240234375,
        "Y": -0.2234458923339844
      },
      {
        "X": 0.2891464233398438,
        "Y": -0.0680618286132812
      },
      {
        "X": 0.0129928588867188,
        "Y": 0.1095085144042969
      }
    ]
  }
}
