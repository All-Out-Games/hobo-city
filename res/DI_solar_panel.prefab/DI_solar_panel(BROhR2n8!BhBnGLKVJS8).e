13
16604343566337
5581868329468 1747850078511731900
{
  "name": "DI_solar_panel",
  "local_enabled": true,
  "local_position": {
    "X": 53.4422721862792969,
    "Y": 7.6976852416992188
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
  "aoid": "5581869004144:1747850078511918800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "solar_panel",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "5581869063472:1747850078511935200",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "solar_panel"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "5581869131656:1747850078511954200",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.5771217346191406,
        "Y": 1.1963653564453125
      },
      {
        "X": -0.5828704833984375,
        "Y": -0.1663284301757812
      },
      {
        "X": 0.7606582641601562,
        "Y": -0.1483154296875000
      },
      {
        "X": 0.7852134704589844,
        "Y": 1.2397384643554688
      }
    ]
  }
}
