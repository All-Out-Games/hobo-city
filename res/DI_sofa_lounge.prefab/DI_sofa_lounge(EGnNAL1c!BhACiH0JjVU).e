13
12141872545793
18046596988252 1747407796366095700
{
  "name": "DI_sofa_lounge",
  "local_enabled": true,
  "local_position": {
    "X": 46.0739364624023438,
    "Y": 4.5780868530273438
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
  "aoid": "18046597417336:1747407796366214400",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "sofa_lounge",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "18046597485196:1747407796366233100",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "sofa_lounge"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "18046597568140:1747407796366256100",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.1665573120117188,
        "Y": -0.0412216186523438
      },
      {
        "X": 1.2102890014648438,
        "Y": -0.0255584716796875
      },
      {
        "X": 1.2563781738281250,
        "Y": 1.1642608642578125
      },
      {
        "X": -1.2097549438476562,
        "Y": 1.1828155517578125
      }
    ]
  }
}
