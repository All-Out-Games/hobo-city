13
15461882265601
777905567220055 1747423697693026300
{
  "name": "DI_Sign_OneWay",
  "local_enabled": true,
  "local_position": {
    "X": -7.2068481445312500,
    "Y": -94.9885253906250000
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "170544753046675:1748581164025041400",
  "next_sibling": "6055263900534:1749748511024844400",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Sign_OneWay.prefab"
},
{
  "cid": 1,
  "aoid": "777905567639195:1747423697693136800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 110,
    "skin": "sign_one_way",
    "RespawnTime": 30,
    "CashReward": 7
  }
},
{
  "cid": 2,
  "aoid": "777905567658081:1747423697693141800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "sign_one_way"
    ],
    "depth_offset": 0.0385437011718750
  }
},
{
  "cid": 3,
  "aoid": "777905567678373:1747423697693147200",
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
