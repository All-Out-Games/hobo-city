13
12064563134465
11426192601166 1747405957383383700
{
  "name": "DI_grave_2",
  "local_enabled": true,
  "local_position": {
    "X": 43.8186874389648438,
    "Y": 0.4696273803710938
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
  "aoid": "11426193088570:1747405957383518300",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "grave_2",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "11426193161542:1747405957383538300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "grave_2"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "11426193244270:1747405957383561300",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.5532455444335938,
        "Y": -0.0100097656250000
      },
      {
        "X": 0.5523834228515625,
        "Y": 0.0012741088867188
      },
      {
        "X": 0.5494155883789062,
        "Y": 0.2477416992187500
      },
      {
        "X": -0.5433197021484375,
        "Y": 0.2528991699218750
      }
    ]
  }
}
