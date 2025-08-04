13
16535624089601
3185764117765 1747849412933545300
{
  "name": "DI_suitcase4",
  "local_enabled": true,
  "local_position": {
    "X": 42.1830673217773438,
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
  "aoid": "3185764640017:1747849412933690000",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "suitcase4",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "3185764699201:1747849412933706300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "suitcase4"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "3185764765765:1747849412933724900",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.2726288139820099,
        "Y": 0.0495910681784153
      },
      {
        "X": 0.0012664796086028,
        "Y": -0.1129989698529243
      },
      {
        "X": 0.2402648925781250,
        "Y": 0.0242385864257812
      },
      {
        "X": -0.0033950805664062,
        "Y": 0.1465301513671875
      }
    ]
  }
}
