14
7129645711364
3895386762
{
  "name": "DI_CarCabrio_60s",
  "local_enabled": true,
  "local_position": {
    "X": 24.2891845703125000,
    "Y": -71.2800598144531250
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 1791137610,
  "next_sibling": 835159602,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarCabrio_60s.prefab"
},
{
  "cid": 1,
  "aoid": 3176360943,
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
  "aoid": 1849957576,
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
  "aoid": 3803374794,
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
