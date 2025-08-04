13
7121055776772
851254892412787 1747353676097910200
{
  "name": "DI_CarCabrio_60s",
  "local_enabled": true,
  "local_position": {
    "X": -4.6211166381835938,
    "Y": -65.2620086669921875
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "previous_sibling": "851876936005923:1747353848428254700",
  "next_sibling": "851254887602443:1747353676096577600",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarCabrio_60s.prefab"
},
{
  "cid": 1,
  "aoid": "851254892616915:1747353676097966700",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "car_cabrio_60s",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": "851254892645345:1747353676097974500",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_cabrio_60s"
    ],
    "depth_offset": 0.4843482971191406
  }
},
{
  "cid": 3,
  "aoid": "851254892694309:1747353676097988100",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.2150268554687500,
        "Y": 0.0233497619628906
      },
      {
        "X": 1.2167968750000000,
        "Y": 0.0167045593261719
      },
      {
        "X": 1.5209808349609375,
        "Y": 0.2187290191650391
      },
      {
        "X": 1.5720672607421875,
        "Y": 1.1426944732666016
      },
      {
        "X": -1.5620422363281250,
        "Y": 1.0444240570068359
      },
      {
        "X": -1.5469512939453125,
        "Y": 0.1925086975097656
      }
    ]
  }
}
