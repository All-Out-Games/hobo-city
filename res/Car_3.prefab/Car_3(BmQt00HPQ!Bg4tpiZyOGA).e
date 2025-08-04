13
146028888065
449749460218832 1745345622137430400
{
  "name": "Car_3",
  "local_enabled": true,
  "local_position": {

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
  "aoid": "449749460463986:1745345622137488700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_3",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "449749460525768:1745345622137503300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "car_3"
    ],
    "depth_offset": 0.5000000000000000
  }
},
{
  "cid": 3,
  "aoid": "449749460601830:1745345622137521400",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -1.3606338500976562,
        "Y": 0.0275612622499466
      },
      {
        "X": 1.4001464843750000,
        "Y": 0.0325014144182205
      },
      {
        "X": 1.4824752807617188,
        "Y": 0.9831835031509399
      },
      {
        "X": -1.4870147705078125,
        "Y": 0.9802619814872742
      }
    ]
  }
}
