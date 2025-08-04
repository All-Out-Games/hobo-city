13
111669149697
402522073614059 1745332371870178100
{
  "name": "Bus",
  "local_enabled": true,
  "local_position": {

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
  "aoid": "402522073841867:1745332371870232300",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "truck_1",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "402522073900877:1745332371870246300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "bus"
    ],
    "depth_offset": 0.5000000000000000
  }
},
{
  "cid": 3,
  "aoid": "402522073981853:1745332371870265600",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -1.9497737884521484,
        "Y": 0.0031746625900269
      },
      {
        "X": 1.9498600959777832,
        "Y": -0.0028229951858521
      },
      {
        "X": 1.9516171216964722,
        "Y": 1.4875148534774780
      },
      {
        "X": -1.9538009166717529,
        "Y": 1.4807509183883667
      }
    ]
  }
}
