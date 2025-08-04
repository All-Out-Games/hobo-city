13
6485400616963
852717350602179 1747354081255887300
{
  "name": "DI_Sign_Stop",
  "local_enabled": true,
  "local_position": {
    "X": 33.8661346435546875,
    "Y": -47.4507408142089844
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "573766931991095:1745626734558468100",
  "next_sibling": "852717352164615:1747354081256319600",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Sign_Stop.prefab"
},
{
  "cid": 1,
  "aoid": "852717350834969:1747354081255951000",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 110,
    "skin": "sign_stop",
    "RespawnTime": 30,
    "CashReward": 7
  }
},
{
  "cid": 2,
  "aoid": "852717350873879:1747354081255961800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "sign_stop"
    ],
    "depth_offset": 0.0385437011718750
  }
},
{
  "cid": 3,
  "aoid": "852717350911767:1747354081255972300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.1120910644531250,
        "Y": 0.0311470031738281
      },
      {
        "X": -0.0689392089843750,
        "Y": -0.0400428771972656
      },
      {
        "X": 0.0592498779296875,
        "Y": -0.0388622283935547
      },
      {
        "X": 0.0926971435546875,
        "Y": 0.0323867797851562
      },
      {
        "X": 0.0557556152343750,
        "Y": 0.1022624969482422
      },
      {
        "X": -0.0718383789062500,
        "Y": 0.1037044525146484
      }
    ]
  }
}
