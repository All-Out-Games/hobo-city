13
16475494547457
1992556800925 1747849081490214200
{
  "name": "DI_cart1",
  "local_enabled": true,
  "local_position": {
    "X": 50.3713607788085938,
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
  "aoid": "1992557335669:1747849081490362000",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "cart1",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "1992557401369:1747849081490380300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "cart1"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "1992557478229:1747849081490401600",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.3207168579101562,
        "Y": 0.1070556640625000
      },
      {
        "X": 0.1969833374023438,
        "Y": -0.1552047729492188
      },
      {
        "X": 0.4717864990234375,
        "Y": 0.0020217895507812
      },
      {
        "X": 0.0817337036132812,
        "Y": 0.3052978515625000
      }
    ]
  }
}
