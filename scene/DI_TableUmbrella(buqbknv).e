14
6227702579202
1761651125
{
  "name": "DI_TableUmbrella",
  "local_enabled": true,
  "local_position": {
    "X": 13.2006683349609375,
    "Y": -52.2086639404296875
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 1231429200,
  "next_sibling": 2949861296,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_TableUmbrella.prefab"
},
{
  "cid": 1,
  "aoid": 2323700024,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "table_umbrella",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": 2337425301,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "table_umbrella"
    ],
    "depth_offset": 1.0464859008789062
  }
},
{
  "cid": 3,
  "aoid": 2972829020,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.9463653564453125,
        "Y": 0.4621124267578125
      },
      {
        "X": -0.2532882690429688,
        "Y": -0.0679855346679688
      },
      {
        "X": 0.2891464233398438,
        "Y": -0.0680618286132812
      },
      {
        "X": 1.0118637084960938,
        "Y": 0.3958816528320312
      },
      {
        "X": 1.0261459350585938,
        "Y": 1.2000350952148438
      },
      {
        "X": 0.6705322265625000,
        "Y": 1.5145606994628906
      },
      {
        "X": -0.7705688476562500,
        "Y": 1.5285644531250000
      },
      {
        "X": -0.9461898803710938,
        "Y": 1.3718605041503906
      }
    ]
  }
}
