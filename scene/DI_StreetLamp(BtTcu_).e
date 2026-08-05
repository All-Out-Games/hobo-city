14
6468220747778
1833814974
{
  "name": "DI_StreetLamp",
  "local_enabled": true,
  "local_position": {
    "X": 13.9793624877929688,
    "Y": -47.3374023437500000
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 4114343558,
  "next_sibling": 1105611217,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_StreetLamp.prefab"
},
{
  "cid": 1,
  "aoid": 1664039827,
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
  "aoid": 2459411019,
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
  "aoid": 491343885,
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
