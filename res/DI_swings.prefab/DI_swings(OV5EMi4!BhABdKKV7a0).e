13
11944304050177
985578653880 1747403057242027700
{
  "name": "DI_swings",
  "local_enabled": true,
  "local_position": {
    "X": 37.4987640380859375,
    "Y": -35.7206916809082031
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
  "aoid": "985579109748:1747403057242153700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "swings",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "985579188624:1747403057242175400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "swings"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "985579289100:1747403057242203300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -1.9950103759765625,
        "Y": -0.0841407775878906
      },
      {
        "X": 1.9889297485351562,
        "Y": -0.0414924621582031
      },
      {
        "X": 1.9443817138671875,
        "Y": 0.9806976318359375
      },
      {
        "X": -1.9800872802734375,
        "Y": 1.0034065246582031
      }
    ]
  }
}
