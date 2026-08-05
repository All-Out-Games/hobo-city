14
3354369458179
1538739296
{
  "name": "DI_FireHydrant",
  "local_enabled": true,
  "local_position": {
    "X": -8.4652023315429688,
    "Y": -81.6743316650390625
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 2915484701,
  "next_sibling": 1795531763,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FireHydrant.prefab"
},
{
  "cid": 1,
  "aoid": 4290774529,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 50,
    "skin": "fire_hydrant",
    "RespawnTime": 30,
    "CashReward": 3
  }
},
{
  "cid": 2,
  "aoid": 2747100669,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "fire_hydrant"
    ],
    "depth_offset": 0.1158313751220703
  }
},
{
  "cid": 3,
  "aoid": 3519966309,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.2196350246667862,
        "Y": -0.0196819324046373
      },
      {
        "X": 0.0046844487078488,
        "Y": -0.1021881178021431
      },
      {
        "X": 0.2270050197839737,
        "Y": -0.0206699389964342
      },
      {
        "X": 0.2326507717370987,
        "Y": 0.1740894466638565
      },
      {
        "X": 0.0017852784367278,
        "Y": 0.2408962398767471
      },
      {
        "X": -0.2223816066980362,
        "Y": 0.1718578487634659
      }
    ]
  }
}
