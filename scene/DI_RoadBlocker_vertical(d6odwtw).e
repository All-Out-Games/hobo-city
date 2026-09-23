14
115964116994
4242659958
{
  "name": "DI_RoadBlocker_vertical",
  "local_enabled": true,
  "local_position": {
    "X": 54.1771049499511719,
    "Y": -70.0214843750000000
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 3252029937,
  "next_sibling": 2731309317,
  "parent": 375284184,
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": 2923415440,
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
  "aoid": 1928839725,
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
  "aoid": 1463632567,
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
