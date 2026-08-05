14
11961483919361
1775078498
{
  "name": "DI_table_diner",
  "local_enabled": true,
  "local_position": {
    "X": -25.3061981201171875,
    "Y": -55.5362701416015625
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 2156880796,
  "next_sibling": 142707330,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_table_diner.prefab"
},
{
  "cid": 1,
  "aoid": 2418349695,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "table_diner",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": 1081008587,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "table_diner"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": 565741483,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.5160255432128906,
        "Y": 0.1261742115020752
      },
      {
        "X": 0.5398445129394531,
        "Y": 0.0793750286102295
      },
      {
        "X": 0.4685325622558594,
        "Y": 0.6611900329589844
      },
      {
        "X": -0.4821853637695312,
        "Y": 0.6493493318557739
      }
    ]
  }
}
