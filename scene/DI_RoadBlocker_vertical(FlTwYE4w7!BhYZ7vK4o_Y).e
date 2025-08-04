13
1279900254209
1571460218063931 1754266111121199000
{
  "name": "DI_RoadBlocker_vertical",
  "local_enabled": true,
  "local_position": {
    "X": 54.3468322753906250,
    "Y": -46.5081024169921875
  },
  "local_rotation": -46.2067718505859375,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "1571455854673783:1754266110104540200",
  "next_sibling": "1571471090643839:1754266113654482200",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_RoadBlocker_vertical.prefab"
},
{
  "cid": 1,
  "aoid": "1571460218430119:1754266111121283900",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 50,
    "skin": "road_blocker_vertical",
    "RespawnTime": 60,
    "CashReward": 3
  }
},
{
  "cid": 2,
  "aoid": "1571460218494103:1754266111121298500",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "road_blocker_vertical"
    ],
    "depth_offset": 1.0324993133544922
  }
},
{
  "cid": 3,
  "aoid": "1571460218583887:1754266111121319400",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.1809539943933487,
        "Y": 0.0235786456614733
      },
      {
        "X": 0.2952881157398224,
        "Y": 0.0278625506907701
      },
      {
        "X": 0.2017517238855362,
        "Y": 1.2823640108108521
      },
      {
        "X": -0.2757110893726349,
        "Y": 1.2852879762649536
      }
    ]
  }
}
