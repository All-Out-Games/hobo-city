13
16509854285825
2862623808676 1747849323173200400
{
  "name": "DI_suitcase1",
  "local_enabled": true,
  "local_position": {
    "X": 40.5171051025390625,
    "Y": -2.9518356323242188
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
  "aoid": "2862624460816:1747849323173380900",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "suitcase1",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "2862624549700:1747849323173405600",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "suitcase1"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "2862624649564:1747849323173433300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2093734741210938,
        "Y": 0.0561370849609375
      },
      {
        "X": 0.0754241943359375,
        "Y": -0.0933685302734375
      },
      {
        "X": 0.2402648925781250,
        "Y": 0.0242385864257812
      },
      {
        "X": -0.0033950805664062,
        "Y": 0.1465301513671875
      }
    ]
  }
}
