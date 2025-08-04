13
12030203396097
9065268674524 1747405301577781400
{
  "name": "DI_firetruck",
  "local_enabled": true,
  "local_position": {
    "X": 48.8384323120117188,
    "Y": -1.2806320190429688
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
  "aoid": "9065269114804:1747405301577902800",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "firetruck",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "9065269183096:1747405301577921700",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "firetruck"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "9065269266796:1747405301577944900",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.8776016235351562,
        "Y": -0.2530593872070312
      },
      {
        "X": 0.8530578613281250,
        "Y": -0.2676315307617188
      },
      {
        "X": 0.8309249877929688,
        "Y": 1.6586761474609375
      },
      {
        "X": -0.8602752685546875,
        "Y": 1.7149887084960938
      }
    ]
  }
}
