14
7146825580547
1935002448
{
  "name": "DI_CarLamdo_30s",
  "local_enabled": true,
  "local_position": {
    "X": 38.2334060668945312,
    "Y": -43.7471084594726562
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "previous_sibling": 2253129714,
  "next_sibling": 3150956938,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarLamdo_30s.prefab"
},
{
  "cid": 1,
  "aoid": 3360942323,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "car_lamdo_30s",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": 4029090953,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_lando_30s"
    ],
    "depth_offset": 0.5804519653320312
  }
},
{
  "cid": 3,
  "aoid": 3102522192,
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
        "X": 1.4487609863281250,
        "Y": 0.0115680694580078
      },
      {
        "X": 1.6845550537109375,
        "Y": 0.1616554260253906
      },
      {
        "X": 1.7254028320312500,
        "Y": 1.2005710601806641
      },
      {
        "X": -1.6316375732421875,
        "Y": 1.1832981109619141
      },
      {
        "X": -1.6282958984375000,
        "Y": 0.2058582305908203
      }
    ]
  }
}
