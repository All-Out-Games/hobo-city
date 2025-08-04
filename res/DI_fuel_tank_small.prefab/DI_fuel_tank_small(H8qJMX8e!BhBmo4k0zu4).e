13
16458314678273
34954745577246 1747848065690319800
{
  "name": "DI_fuel_tank_small",
  "local_enabled": true,
  "local_position": {
    "X": 47.0836715698242188,
    "Y": 10.2728805541992188
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
  "aoid": "34954746095826:1747848065690463100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "fuel_tank_small",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "34954746173622:1747848065690484800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "fuel_tank_small"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "34954746252750:1747848065690506700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.9869232177734375,
        "Y": 0.0144348144531250
      },
      {
        "X": -0.8249588012695312,
        "Y": -0.2716674804687500
      },
      {
        "X": -0.5531921386718750,
        "Y": -0.4124374389648438
      },
      {
        "X": -0.0526123046875000,
        "Y": -0.5110473632812500
      },
      {
        "X": 0.5086288452148438,
        "Y": -0.4093933105468750
      },
      {
        "X": 0.7911148071289062,
        "Y": -0.2299194335937500
      },
      {
        "X": 0.9135971069335938,
        "Y": -0.0027313232421875
      },
      {
        "X": 0.9253158569335938,
        "Y": 0.6964721679687500
      },
      {
        "X": -0.9665985107421875,
        "Y": 0.6709213256835938
      }
    ]
  }
}
