13
120259084289
443261482375184 1745344074480436400
{
  "name": "Bush",
  "local_enabled": true,
  "local_position": {
    "X": -92.3956832885742188
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
  "aoid": "443261482619372:1745344074480494500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "bush",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "443261482677542:1745344074480508300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "bush"
    ]
  }
},
{
  "cid": 3,
  "aoid": "443261482760660:1745344074480528100",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.6015091538429260,
        "Y": -0.0445227064192295
      },
      {
        "X": 0.5652771592140198,
        "Y": -0.0399942733347416
      },
      {
        "X": 0.6622238755226135,
        "Y": 0.1543467491865158
      },
      {
        "X": 0.5607605576515198,
        "Y": 0.3026374876499176
      },
      {
        "X": -0.5969772934913635,
        "Y": 0.3026374876499176
      },
      {
        "X": -0.6851273179054260,
        "Y": 0.1451130360364914
      }
    ]
  }
}
