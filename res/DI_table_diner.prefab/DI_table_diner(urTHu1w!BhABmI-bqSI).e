13
11961483919361
3207587622256 1747403674460529800
{
  "name": "DI_table_diner",
  "local_enabled": true,
  "local_position": {
    "X": 36.7000350952148438,
    "Y": -1.3473434448242188
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
  "aoid": "3207588083020:1747403674460657300",
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
  "aoid": "3207588149836:1747403674460675600",
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
  "aoid": "3207588232960:1747403674460698700",
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
