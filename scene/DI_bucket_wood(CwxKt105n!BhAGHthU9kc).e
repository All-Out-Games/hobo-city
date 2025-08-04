13
15384572854273
777434946817639 1747423573612681500
{
  "name": "DI_bucket_wood",
  "local_enabled": true,
  "local_position": {
    "X": 25.4311294555664062,
    "Y": -56.8359756469726562
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "777215092289851:1747423515647438900",
  "next_sibling": "109833749462640:1748567018546556400",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_bucket_wood.prefab"
},
{
  "cid": 1,
  "aoid": "777434947123919:1747423573612761600",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "bucket_wood",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "777434947179931:1747423573612776400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "bucket_wood"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 3,
  "aoid": "777434947260719:1747423573612797800",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.1485595703125000,
        "Y": -0.0569000244140625
      },
      {
        "X": 0.1515655517578125,
        "Y": -0.0576629638671875
      },
      {
        "X": 0.1645202636718750,
        "Y": 0.0922088623046875
      },
      {
        "X": -0.1711730957031250,
        "Y": 0.1024093627929688
      }
    ]
  }
}
