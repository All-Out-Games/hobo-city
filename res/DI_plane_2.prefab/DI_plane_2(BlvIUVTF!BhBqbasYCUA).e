13
16750372454401
6991272563909 1747864732437587200
{
  "name": "DI_plane_2",
  "local_enabled": true,
  "local_position": {
    "X": 51.3543319702148438,
    "Y": 26.9959945678710938
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
  "aoid": "6991273201325:1747864732437763600",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 750,
    "skin": "plane_2",
    "RespawnTime": 120,
    "CashReward": 350
  }
},
{
  "cid": 2,
  "aoid": "6991273267313:1747864732437781900",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-big/016ARP_Destructible_Items_Big.spine",
    "ordered_skins": [
      "plane_2"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "6991273338773:1747864732437801700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": 3.1516685485839844,
        "Y": -0.4629211425781250
      },
      {
        "X": 3.1778373718261719,
        "Y": 0.9310989379882812
      },
      {
        "X": -4.8554840087890625,
        "Y": 0.8552169799804688
      },
      {
        "X": -4.8509063720703125,
        "Y": -0.4062652587890625
      }
    ]
  }
}
