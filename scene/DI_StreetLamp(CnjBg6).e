14
6571299962882
2810976314
{
  "name": "DI_StreetLamp",
  "local_enabled": true,
  "local_position": {
    "X": -18.4782943725585938,
    "Y": -81.3216171264648438
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 3882014477,
  "next_sibling": 463923276,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_StreetLamp.prefab"
},
{
  "cid": 1,
  "aoid": 326154155,
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
  "aoid": 2332164356,
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
  "aoid": 893629571,
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
