13
3320009719810
952269338558135 1748286570073562500
{
  "name": "DI_Tree_1",
  "local_enabled": false,
  "local_position": {
    "X": 75.7059631347656250,
    "Y": -50.2359733581542969
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "109833765954104:1748567018550398700",
  "next_sibling": "990439468776836:1748554475084108200",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Tree_1.prefab"
},
{
  "cid": 1,
  "aoid": "952269339129997:1748286570073712700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 260,
    "skin": "tree_1",
    "RespawnTime": 30,
    "CashReward": 18
  }
},
{
  "cid": 2,
  "aoid": "952269339201209:1748286570073731400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "tree_1"
    ],
    "depth_offset": 0.1687088012695312
  }
},
{
  "cid": 3,
  "aoid": "952269339269495:1748286570073749400",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.1917877197265625,
        "Y": 0.1400566101074219
      },
      {
        "X": -0.1153717041015625,
        "Y": 0.0179367065429688
      },
      {
        "X": 0.1595687866210938,
        "Y": 0.0141181945800781
      },
      {
        "X": 0.2456359863281250,
        "Y": 0.1369590759277344
      },
      {
        "X": 0.1220397949218750,
        "Y": 0.3380165100097656
      },
      {
        "X": -0.0919952392578125,
        "Y": 0.3418350219726562
      }
    ]
  }
}
