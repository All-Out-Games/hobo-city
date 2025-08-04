13
16612933500929
5852646852433 1747850153727274500
{
  "name": "DI_toolbox",
  "local_enabled": true,
  "local_position": {
    "X": 55.3208160400390625,
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
  "aoid": "5852647406401:1747850153727427800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "toolbox",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "5852647466485:1747850153727444500",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "toolbox"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "5852647538665:1747850153727464500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.6279716491699219,
        "Y": 0.3164672851562500
      },
      {
        "X": -0.6337203979492188,
        "Y": -0.0137481689453125
      },
      {
        "X": 0.6386184692382812,
        "Y": -0.0008239746093750
      },
      {
        "X": 0.6275787353515625,
        "Y": 0.3242416381835938
      }
    ]
  }
}
