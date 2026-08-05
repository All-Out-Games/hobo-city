14
6854767804419
409092037
{
  "name": "DI_TrashCanPublic",
  "local_enabled": true,
  "local_position": {
    "X": 9.1199874877929688,
    "Y": -94.3020477294921875
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": 659083039,
  "next_sibling": 1054400004,
  "parent": 375284184,
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_TrashCanPublic.prefab"
},
{
  "cid": 1,
  "aoid": 3628620868,
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 50,
    "skin": "trash_can_public",
    "RespawnTime": 30,
    "CashReward": 3
  }
},
{
  "cid": 2,
  "aoid": 2524249045,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "trash_can_public"
    ],
    "depth_offset": 0.1124954223632812
  }
},
{
  "cid": 3,
  "aoid": 2347292994,
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.3252716064453125,
        "Y": 0.0527114868164062
      },
      {
        "X": -0.1681137084960938,
        "Y": -0.0710563659667969
      },
      {
        "X": 0.1793441772460938,
        "Y": -0.0699272155761719
      },
      {
        "X": 0.3230895996093750,
        "Y": 0.0595016479492188
      },
      {
        "X": 0.3290557861328125,
        "Y": 0.1842842102050781
      },
      {
        "X": 0.1879577636718750,
        "Y": 0.3100013732910156
      },
      {
        "X": -0.1842880249023438,
        "Y": 0.3105239868164062
      },
      {
        "X": -0.3260040283203125,
        "Y": 0.1846694946289062
      }
    ]
  }
}
