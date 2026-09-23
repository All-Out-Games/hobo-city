14
3766686318596
657466936
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
  "previous_sibling": 1752400381,
  "next_sibling": 2752311151,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_RoadBlocker_vertical.prefab"
},
{
  "cid": 1,
  "aoid": 703692559,
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
  "aoid": 3643705495,
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
  "aoid": 4029956183,
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
