13
16569983827969
4489507139821 1747849775080947900
{
  "name": "DI_coffee",
  "local_enabled": true,
  "local_position": {
    "X": 52.2743606567382812,
    "Y": 4.7091598510742188
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
  "aoid": "4489507640005:1747849775081086200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "coffee",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "4489507700881:1747849775081103100",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "coffee"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "4489507782133:1747849775081125700",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.9763565063476562,
        "Y": -0.1075134277343750
      },
      {
        "X": 1.1798057556152344,
        "Y": -0.1005401611328125
      },
      {
        "X": 1.1813354492187500,
        "Y": 0.3902587890625000
      },
      {
        "X": -0.9706649780273438,
        "Y": 0.3772964477539062
      }
    ]
  }
}
