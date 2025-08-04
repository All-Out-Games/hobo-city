13
163208757249
451119078995961 1745345948849409100
{
  "name": "Dumpster",
  "local_enabled": true,
  "local_position": {
    "X": -73.0302734375000000
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
  "aoid": "451119079221333:1745345948849462700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "dumpster",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "451119079280973:1745345948849476800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "dumpster"
    ],
    "depth_offset": 0.1000000014901161
  }
},
{
  "cid": 3,
  "aoid": "451119079360059:1745345948849495700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.8782272338867188,
        "Y": 0.0062161684036255
      },
      {
        "X": 0.8782272338867188,
        "Y": -0.0066019296646118
      },
      {
        "X": 0.8910522460937500,
        "Y": 0.8654105663299561
      },
      {
        "X": -0.9038619995117188,
        "Y": 0.8525924682617188
      }
    ]
  }
}
