13
12021613461505
9050638816944 1747405297513972700
{
  "name": "DI_police_car",
  "local_enabled": true,
  "local_position": {
    "X": 46.0593566894531250,
    "Y": -1.0469055175781250
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
  "aoid": "9050639272416:1747405297514098600",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "police_car",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "9050639348520:1747405297514119700",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "police_car"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "9050639433300:1747405297514143200",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.5112915039062500,
        "Y": -0.0008850097656250
      },
      {
        "X": -0.0824050903320312,
        "Y": -0.7394332885742188
      },
      {
        "X": 1.5394821166992188,
        "Y": 0.6575775146484375
      },
      {
        "X": 0.1780395507812500,
        "Y": 1.3724975585937500
      }
    ]
  }
}
