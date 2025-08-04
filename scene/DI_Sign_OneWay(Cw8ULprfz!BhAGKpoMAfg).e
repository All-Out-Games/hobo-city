13
15479062134785
778201024870387 1747423775591237600
{
  "name": "DI_Sign_OneWay",
  "local_enabled": true,
  "local_position": {
    "X": 9.0657424926757812,
    "Y": -97.9483795166015625
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "6055263900534:1749748511024844400",
  "next_sibling": "109833756799318:1748567018548266700",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Sign_OneWay.prefab"
},
{
  "cid": 1,
  "aoid": "778201025199847:1747423775591323900",
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
  "aoid": "778201025250501:1747423775591337300",
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
  "aoid": "778201025324753:1747423775591356900",
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
