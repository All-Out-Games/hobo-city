13
12047383265281
10599301193446 1747405727693637000
{
  "name": "DI_grave_1",
  "local_enabled": true,
  "local_position": {
    "X": 42.7386627197265625,
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
  "aoid": "10599301649134:1747405727693763200",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "grave_1",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "10599301717462:1747405727693782000",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "grave_1"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "10599301811242:1747405727693808100",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2326583862304688,
        "Y": -0.0100097656250000
      },
      {
        "X": 0.2259674072265625,
        "Y": 0.0012741088867188
      },
      {
        "X": 0.2492294311523438,
        "Y": 0.2215042114257812
      },
      {
        "X": -0.2518768310546875,
        "Y": 0.2208328247070312
      }
    ]
  }
}
