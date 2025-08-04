13
16492674416641
2486489832406 1747849218692531800
{
  "name": "DI_pole",
  "local_enabled": true,
  "local_position": {
    "X": 52.1244049072265625,
    "Y": 2.1346969604492188
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
  "aoid": "2486490385690:1747849218692684900",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "pole",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "2486490447862:1747849218692702200",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "pole"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "2486490522094:1747849218692722800",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.1522445678710938,
        "Y": 0.1300811767578125
      },
      {
        "X": -0.1273040771484375,
        "Y": -0.0448226928710938
      },
      {
        "X": 0.1488723754882812,
        "Y": -0.0416946411132812
      },
      {
        "X": 0.1564559936523438,
        "Y": 0.1203842163085938
      }
    ]
  }
}
