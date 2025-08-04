13
6347961663489
580938527170723 1745628445284822100
{
  "name": "DI_FenceCopper_vertical",
  "local_enabled": true,
  "local_position": {
    "X": 43.8715515136718750,
    "Y": -39.5255165100097656
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "853242786976814:1747354226822262700",
  "next_sibling": "580950329259805:1745628448100115500",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_FenceCopper_vertical.prefab"
},
{
  "cid": 1,
  "aoid": "580938527414407:1745628445284880100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "fence_copper_vertical",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "580938527477407:1745628445284894900",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "fence_copper_vertical"
    ],
    "depth_offset": 1.7044639587402344
  }
},
{
  "cid": 3,
  "aoid": "580938527559559:1745628445284914500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.0965423583984375,
        "Y": 1.6075839996337891
      },
      {
        "X": -0.1105194091796875,
        "Y": -0.0087661743164062
      },
      {
        "X": 0.0712585449218750,
        "Y": -0.0066986083984375
      },
      {
        "X": 0.0727233886718750,
        "Y": 1.6057415008544922
      }
    ]
  }
}
