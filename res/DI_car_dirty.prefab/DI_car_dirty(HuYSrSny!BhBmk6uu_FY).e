13
16415365005313
33973504649714 1747847793126138200
{
  "name": "DI_car_dirty",
  "local_enabled": true,
  "local_position": {
    "X": 47.0683593750000000,
    "Y": 7.7483291625976562
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
  "aoid": "33973505138738:1747847793126273500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_dirty",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "33973505226758:1747847793126297900",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_dirty"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "33973505296418:1747847793126317300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.0332107543945312,
        "Y": -0.1447982788085938
      },
      {
        "X": 1.2485427856445312,
        "Y": -0.1048278808593750
      },
      {
        "X": 1.3508758544921875,
        "Y": 0.8269271850585938
      },
      {
        "X": -1.0105209350585938,
        "Y": 0.8607330322265625
      }
    ]
  }
}
