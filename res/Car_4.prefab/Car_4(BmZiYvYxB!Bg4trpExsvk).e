13
154618822657
450355653348417 1745345766740118500
{
  "name": "Car_4",
  "local_enabled": true,
  "local_position": {
    "X": -77.3619384765625000
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
  "aoid": "450355653596091:1745345766740177400",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_4",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "450355653663501:1745345766740193400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "car_4"
    ],
    "depth_offset": 0.2500000000000000
  }
},
{
  "cid": 3,
  "aoid": "450355653752835:1745345766740214700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -1.0554275512695312,
        "Y": 0.0223190784454346
      },
      {
        "X": 0.8583526611328125,
        "Y": 0.0346361398696899
      },
      {
        "X": 1.1724395751953125,
        "Y": 0.6797552704811096
      },
      {
        "X": -1.0862197875976562,
        "Y": 0.6797555088996887
      }
    ]
  }
}
