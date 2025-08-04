13
2336462209027
6055263150485 1749748511024669600
{
  "name": "DI_Sign_NoEnter",
  "local_enabled": true,
  "local_position": {
    "X": 9.1498107910156250,
    "Y": -89.9948806762695312
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "6055262490263:1749748511024516300",
  "next_sibling": "1121578657829933:1749317273645078200",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Sign_NoEnter.prefab"
},
{
  "cid": 1,
  "aoid": "6055263248998:1749748511024692500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 110,
    "skin": "sign_no_enter",
    "RespawnTime": 30,
    "CashReward": 7
  }
},
{
  "cid": 2,
  "aoid": "6055263265295:1749748511024696300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "sign_no_enter"
    ],
    "depth_offset": 0.0385437011718750
  }
},
{
  "cid": 3,
  "aoid": "6055263283269:1749748511024700500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.1303863525390625,
        "Y": 0.0349979400634766
      },
      {
        "X": -0.0930175781250000,
        "Y": -0.0400428771972656
      },
      {
        "X": 0.0361480712890625,
        "Y": -0.0388622283935547
      },
      {
        "X": 0.0657348632812500,
        "Y": 0.0333499908447266
      },
      {
        "X": 0.0335998535156250,
        "Y": 0.1022624969482422
      },
      {
        "X": -0.0910949707031250,
        "Y": 0.1017780303955078
      }
    ]
  }
}
