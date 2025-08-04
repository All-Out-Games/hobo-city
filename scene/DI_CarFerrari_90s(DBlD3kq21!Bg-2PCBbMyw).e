13
7069516169220
851369750867381 1747353707918183600
{
  "name": "DI_CarFerrari_90s",
  "local_enabled": true,
  "local_position": {
    "X": 16.9518203735351562,
    "Y": -30.7505874633789062
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "170518284433182:1748581157857955000",
  "next_sibling": "851254889267747:1747353676097038900",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_CarFerrari_90s.prefab"
},
{
  "cid": 1,
  "aoid": "851369751101903:1747353707918248200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 240,
    "skin": "car_ferrari_90s",
    "RespawnTime": 30,
    "CashReward": 17
  }
},
{
  "cid": 2,
  "aoid": "851369751140413:1747353707918258800",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "car_ferrari_90s"
    ],
    "depth_offset": 0.7316646575927734
  }
},
{
  "cid": 3,
  "aoid": "851369751180273:1747353707918269900",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.1468811035156250,
        "Y": -0.0028667449951172
      },
      {
        "X": 1.1014404296875000,
        "Y": -0.0252380371093750
      },
      {
        "X": 1.6101226806640625,
        "Y": 0.2711601257324219
      },
      {
        "X": 1.6821746826171875,
        "Y": 1.3209552764892578
      },
      {
        "X": -1.5567932128906250,
        "Y": 1.3118171691894531
      },
      {
        "X": -1.5469512939453125,
        "Y": 0.2973690032958984
      }
    ]
  }
}
