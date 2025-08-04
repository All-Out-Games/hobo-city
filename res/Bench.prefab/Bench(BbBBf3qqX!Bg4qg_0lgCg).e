13
103079215107
400292560415383 1745331840037028000
{
  "name": "Bench",
  "local_enabled": true,
  "local_position": {
    "X": -123.0755920410156250,
    "Y": 10.5975980758666992
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
  "aoid": "400292560665389:1745331840037087500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "truck_1",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "400292560743089:1745331840037105800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "bench"
    ]
  }
},
{
  "cid": 3,
  "aoid": "400292560833977:1745331840037127500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.7876739501953125,
        "Y": -0.0727663040161133
      },
      {
        "X": 0.7947158813476562,
        "Y": -0.0727663040161133
      },
      {
        "X": 0.7841033935546875,
        "Y": 0.6744060516357422
      },
      {
        "X": -0.7806091308593750,
        "Y": 0.6779460906982422
      }
    ]
  }
}
