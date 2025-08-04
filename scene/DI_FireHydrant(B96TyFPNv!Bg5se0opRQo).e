13
3354369458179
553762784867183 1745621962730050600
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
  "previous_sibling": "1305091949578902:1750623497937644800",
  "next_sibling": "310274985097704:1748057157709146300",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FireHydrant.prefab"
},
{
  "cid": 1,
  "aoid": "553762785111749:1745621962730108600",
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
  "aoid": "553762785172271:1745621962730122900",
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
  "aoid": "553762785255053:1745621962730142700",
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
