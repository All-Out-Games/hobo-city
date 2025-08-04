13
137438953473
449108303918517 1745345469194548000
{
  "name": "Car_2",
  "local_enabled": true,
  "local_position": {
    "X": -86.4949417114257812
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
  "aoid": "449108304152817:1745345469194603800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_2",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "449108304223209:1745345469194620400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "car_2"
    ]
  }
},
{
  "cid": 3,
  "aoid": "449108304299901:1745345469194638600",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -1.8009796142578125,
        "Y": -0.0014405846595764
      },
      {
        "X": 1.9771957397460938,
        "Y": -0.0121203660964966
      },
      {
        "X": 2.0038986206054688,
        "Y": 0.9305811524391174
      },
      {
        "X": -1.8854293823242188,
        "Y": 0.9446573853492737
      },
      {
        "X": -2.0359497070312500,
        "Y": 0.3859126269817352
      }
    ]
  }
}
