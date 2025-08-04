13
12124692676609
15754375533456 1747407159644321800
{
  "name": "DI_beach_umbrella",
  "local_enabled": true,
  "local_position": {
    "X": 40.3141479492187500,
    "Y": 4.2335739135742188
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": "15754376151576:1747407159644492600",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "beach_umbrella",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "15754376225268:1747407159644512800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "beach_umbrella"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "15754376306808:1747407159644535500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2667846679687500,
        "Y": -0.0944213867187500
      },
      {
        "X": 0.2586517333984375,
        "Y": -0.0881576538085938
      },
      {
        "X": 0.2025299072265625,
        "Y": 0.2782974243164062
      },
      {
        "X": -0.2290191650390625,
        "Y": 0.2617797851562500
      }
    ]
  }
}
