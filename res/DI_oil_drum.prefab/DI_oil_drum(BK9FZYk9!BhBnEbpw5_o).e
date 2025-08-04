13
16595753631745
5150830070077 1747849958780018600
{
  "name": "DI_oil_drum",
  "local_enabled": true,
  "local_position": {
    "X": 56.1228828430175781,
    "Y": 4.6439285278320312
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
  "aoid": "5150830611733:1747849958780168300",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "oil_drum",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "5150830675273:1747849958780186000",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "oil_drum"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "5150830751305:1747849958780207100",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2172698974609375,
        "Y": 0.0070190429687500
      },
      {
        "X": -0.0967292785644531,
        "Y": -0.0592727661132812
      },
      {
        "X": 0.0951118469238281,
        "Y": -0.1037368774414062
      },
      {
        "X": 0.2738914489746094,
        "Y": -0.0660934448242188
      },
      {
        "X": 0.4060211181640625,
        "Y": 0.0081787109375000
      },
      {
        "X": 0.3221282958984375,
        "Y": 0.1307067871093750
      },
      {
        "X": 0.0811538696289062,
        "Y": 0.1599426269531250
      },
      {
        "X": -0.1636161804199219,
        "Y": 0.1349868774414062
      }
    ]
  }
}
