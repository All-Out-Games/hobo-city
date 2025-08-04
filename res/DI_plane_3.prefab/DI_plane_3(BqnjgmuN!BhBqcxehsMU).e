13
16758962388993
7326736214925 1747864825620775700
{
  "name": "DI_plane_3",
  "local_enabled": true,
  "local_position": {
    "X": 51.3543319702148438,
    "Y": 17.2648544311523438
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
  "aoid": "7326736725405:1747864825620917000",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 1500,
    "skin": "plane_3",
    "RespawnTime": 5,
    "CashReward": 300
  }
},
{
  "cid": 2,
  "aoid": "7326736788081:1747864825620934400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-big/016ARP_Destructible_Items_Big.spine",
    "ordered_skins": [
      "plane_3"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "7326736858533:1747864825620954000",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": 2.2007715702056885,
        "Y": -0.3836593925952911
      },
      {
        "X": 2.1873209476470947,
        "Y": 0.6801071763038635
      },
      {
        "X": -3.3498995304107666,
        "Y": 0.7627487778663635
      },
      {
        "X": -3.3189089298248291,
        "Y": -0.3930588066577911
      }
    ]
  }
}
