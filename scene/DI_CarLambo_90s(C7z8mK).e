14
7078106103811
3150956938
{
  "name": "DI_CarLambo_90s",
  "local_enabled": true,
  "local_position": {
    "X": 46.2770919799804688,
    "Y": -43.9374542236328125
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "previous_sibling": 1935002448,
  "next_sibling": 3755702784,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarLambo_90s.prefab"
},
{
  "cid": 1,
  "aoid": 2264663426,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "car_lambo_90s",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": 2229496143,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_lambo_90s"
    ],
    "depth_offset": 0.6652164459228516
  }
},
{
  "cid": 3,
  "aoid": 4122545537,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.0599517822265625,
        "Y": 0.0087242126464844
      },
      {
        "X": 1.0318908691406250,
        "Y": 0.0674896240234375
      },
      {
        "X": 1.4710235595703125,
        "Y": 0.2074108123779297
      },
      {
        "X": 1.5372924804687500,
        "Y": 1.2514095306396484
      },
      {
        "X": -1.5452117919921875,
        "Y": 1.3002262115478516
      },
      {
        "X": -1.5469512939453125,
        "Y": 0.2973690032958984
      }
    ]
  }
}
