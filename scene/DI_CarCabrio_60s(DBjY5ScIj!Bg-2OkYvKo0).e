13
7103875907588
851254889267747 1747353676097038900
{
  "name": "DI_CarCabrio_60s",
  "local_enabled": true,
  "local_position": {
    "X": 19.8668289184570312,
    "Y": -46.6412429809570312
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "851369750867381:1747353707918183600",
  "next_sibling": "851254894094159:1747353676098376000",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarCabrio_60s.prefab"
},
{
  "cid": 1,
  "aoid": "851254889477671:1747353676097096900",
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
  "aoid": "851254889506923:1747353676097105000",
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
  "aoid": "851254889541259:1747353676097114600",
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
