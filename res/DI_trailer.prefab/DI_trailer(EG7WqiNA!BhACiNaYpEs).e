13
12150462480385
18068233921344 1747407802376294700
{
  "name": "DI_trailer",
  "local_enabled": true,
  "local_position": {
    "X": 49.2679672241210938,
    "Y": 4.9304351806640625
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
  "aoid": "18068234921280:1747407802376571700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "trailer",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "18068234990148:1747407802376590600",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "trailer"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "18068235070464:1747407802376612900",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.0443572998046875,
        "Y": -0.4259824752807617
      },
      {
        "X": 1.2361183166503906,
        "Y": 0.6829862594604492
      },
      {
        "X": 1.2757987976074219,
        "Y": 1.0855021476745605
      },
      {
        "X": 1.6080360412597656,
        "Y": 1.4027318954467773
      },
      {
        "X": 0.1464462280273438,
        "Y": 1.2728576660156250
      },
      {
        "X": -1.3296127319335938,
        "Y": 0.1604647636413574
      }
    ]
  }
}
