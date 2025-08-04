13
7078106103811
851411979004255 1747353719617025200
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
  "previous_sibling": "851456936354805:1747353732071965000",
  "next_sibling": "851534036942669:1747353753431836100",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarLambo_90s.prefab"
},
{
  "cid": 1,
  "aoid": "851411979233891:1747353719617088300",
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
  "aoid": "851411979288963:1747353719617103500",
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
  "aoid": "851411979330819:1747353719617115200",
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
