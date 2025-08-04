13
16527034155009
3174725855368 1747849409867390400
{
  "name": "DI_suitcase3",
  "local_enabled": true,
  "local_position": {
    "X": 41.5909118652343750,
    "Y": -2.9518356323242188
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
  "aoid": "3174726370960:1747849409867533100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "suitcase3",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "3174726431908:1747849409867550000",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "suitcase3"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "3174726503476:1747849409867570000",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2137374877929688,
        "Y": 0.0517730712890625
      },
      {
        "X": -0.0270919799804688,
        "Y": -0.0802841186523438
      },
      {
        "X": 0.2402648925781250,
        "Y": 0.0635070800781250
      },
      {
        "X": 0.0663986206054688,
        "Y": 0.1792526245117188
      }
    ]
  }
}
