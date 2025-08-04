13
16449724743681
34950006502858 1747848064373923400
{
  "name": "DI_fuel_tank_large",
  "local_enabled": true,
  "local_position": {
    "X": 44.2546157836914062,
    "Y": 10.8943176269531250
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
  "aoid": "34950007213246:1747848064374120100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "fuel_tank_large",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "34950007277614:1747848064374138000",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "fuel_tank_large"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "34950007346338:1747848064374157000",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.5128631591796875,
        "Y": -0.0898208618164062
      },
      {
        "X": -1.3383255004882812,
        "Y": -0.4106369018554688
      },
      {
        "X": -0.9790420532226562,
        "Y": -0.6386260986328125
      },
      {
        "X": -0.5599060058593750,
        "Y": -0.8630523681640625
      },
      {
        "X": 0.0751266479492188,
        "Y": -0.9132080078125000
      },
      {
        "X": 0.6356735229492188,
        "Y": -0.8351898193359375
      },
      {
        "X": 1.1559371948242188,
        "Y": -0.6021194458007812
      },
      {
        "X": 1.4988861083984375,
        "Y": -0.3814620971679688
      },
      {
        "X": 1.7303543090820312,
        "Y": -0.0410232543945312
      },
      {
        "X": 1.6770401000976562,
        "Y": 0.8692626953125000
      },
      {
        "X": -1.5433120727539062,
        "Y": 0.8087768554687500
      }
    ]
  }
}
