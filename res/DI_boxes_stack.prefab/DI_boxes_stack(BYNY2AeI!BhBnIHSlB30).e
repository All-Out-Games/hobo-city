13
16621523435521
6061689407368 1747850211794099700
{
  "name": "DI_boxes_stack",
  "local_enabled": true,
  "local_position": {
    "X": 56.8064689636230469,
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
  "aoid": "6061689961264:1747850211794253100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "boxes_stack",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "6061690025812:1747850211794270900",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "boxes_stack"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "6061690099396:1747850211794291400",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.1141891479492188,
        "Y": 0.3665237426757812
      },
      {
        "X": -0.7004432678222656,
        "Y": 0.0630035400390625
      },
      {
        "X": 0.0147399902343750,
        "Y": -0.2444305419921875
      },
      {
        "X": 0.7209930419921875,
        "Y": 0.0839767456054688
      }
    ]
  }
}
