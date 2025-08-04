13
5789615915011
850919686511314 1747353583232797600
{
  "name": "DI_Tree_1",
  "local_enabled": true,
  "local_position": {
    "X": -34.7062149047851562,
    "Y": -43.8647308349609375
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "850919677831479:1747353583230392800",
  "next_sibling": "749117769939194:1749610577416077700",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_Tree_1.prefab"
},
{
  "cid": 1,
  "aoid": "850919686794704:1747353583232875700",
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
  "aoid": "850919686835334:1747353583232887000",
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
  "aoid": "850919686884222:1747353583232900500",
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
