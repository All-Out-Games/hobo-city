13
16406775070721
33966324541622 1747847791131683800
{
  "name": "DI_car_taxi",
  "local_enabled": true,
  "local_position": {
    "X": 43.7690429687500000,
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
  "aoid": "33966325042850:1747847791131822500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_taxi",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "33966325130042:1747847791131846700",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_taxi"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "33966325196102:1747847791131865100",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.8094787597656250,
        "Y": -0.1031646728515625
      },
      {
        "X": 1.5457229614257812,
        "Y": -0.1265792846679688
      },
      {
        "X": 1.5103302001953125,
        "Y": 0.8051757812500000
      },
      {
        "X": -1.8121185302734375,
        "Y": 0.7954788208007812
      }
    ]
  }
}
