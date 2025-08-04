13
16398185136129
33948968008937 1747847786310473400
{
  "name": "DI_car_tesla_red",
  "local_enabled": true,
  "local_position": {
    "X": 40.1635971069335938,
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
  "aoid": "33948968490221:1747847786310606400",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_tesla_red",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "33948968578169:1747847786310630900",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_tesla_red"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "33948968645993:1747847786310649700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.3304023742675781,
        "Y": -0.1302947998046875
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
        "X": -1.3149604797363281,
        "Y": 0.7954788208007812
      }
    ]
  }
}
