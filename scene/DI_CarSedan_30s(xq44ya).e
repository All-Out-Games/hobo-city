14
7198365188099
789476096
{
  "name": "DI_CarSedan_30s",
  "local_enabled": true,
  "local_position": {
    "X": -1.1181945800781250,
    "Y": -65.1851806640625000
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "previous_sibling": 3044280421,
  "next_sibling": 763249609,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarSedan_30s.prefab"
},
{
  "cid": 1,
  "aoid": 918453965,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "car_sedan_30s",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": 3836428189,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_sedan_30s"
    ],
    "depth_offset": 0.5804519653320312
  }
},
{
  "cid": 3,
  "aoid": 2065000253,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.2023010253906250,
        "Y": -0.0116119384765625
      },
      {
        "X": 1.0768127441406250,
        "Y": 0.0115680694580078
      },
      {
        "X": 1.3546600341796875,
        "Y": 0.1422500610351562
      },
      {
        "X": 1.3741302490234375,
        "Y": 1.0453262329101562
      },
      {
        "X": -1.4979858398437500,
        "Y": 1.0463466644287109
      },
      {
        "X": -1.4970855712890625,
        "Y": 0.1402492523193359
      }
    ]
  }
}
