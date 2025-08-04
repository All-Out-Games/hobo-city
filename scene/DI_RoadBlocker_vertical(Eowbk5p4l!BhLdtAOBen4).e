13
3766686318596
1305149912292901 1750623516124113400
{
  "name": "DI_RoadBlocker_vertical",
  "local_enabled": true,
  "local_position": {
    "X": -37.1461868286132812,
    "Y": -51.1535491943359375
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "1305149911215353:1750623516123775400",
  "next_sibling": "1305149913233793:1750623516124408700",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_RoadBlocker_vertical.prefab"
},
{
  "cid": 1,
  "aoid": "1305149912471313:1750623516124169300",
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
  "aoid": "1305149912494998:1750623516124176700",
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
  "aoid": "1305149912520133:1750623516124184600",
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
