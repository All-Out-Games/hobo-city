13
5583457484801
575426239582783 1745627130372949300
{
  "name": "DI_TrafficLight",
  "local_enabled": true,
  "local_position": {
    "X": -104.2042465209960938,
    "Y": 37.6356735229492188
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  }
},
{
  "cid": 1,
  "aoid": "575426239844107:1745627130373011500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 190,
    "skin": "traffic_light",
    "RespawnTime": 30,
    "CashReward": 14
  }
},
{
  "cid": 2,
  "aoid": "575426239904839:1745627130373025800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "traffic_light"
    ],
    "depth_offset": 0.0670814514160156
  }
},
{
  "cid": 3,
  "aoid": "575426239984261:1745627130373044800",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.2743377685546875,
        "Y": 0.0818138122558594
      },
      {
        "X": 0.0254211425781250,
        "Y": -0.0914268493652344
      },
      {
        "X": 0.3099899291992188,
        "Y": 0.0725975036621094
      },
      {
        "X": 0.0220184326171875,
        "Y": 0.2526741027832031
      }
    ]
  }
}
