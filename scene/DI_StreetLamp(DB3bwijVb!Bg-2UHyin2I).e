13
6571299962882
852632353256795 1747354057708305800
{
  "name": "DI_StreetLamp",
  "local_enabled": true,
  "local_position": {
    "X": -18.4782943725585938,
    "Y": -81.3216171264648438
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "307101603326231:1750310366711116900",
  "next_sibling": "170518275505178:1748581157855875800",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_StreetLamp.prefab"
},
{
  "cid": 1,
  "aoid": "852632353447387:1747354057708358500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 160,
    "skin": "street_lamp",
    "RespawnTime": 30,
    "CashReward": 10
  }
},
{
  "cid": 2,
  "aoid": "852632353475665:1747354057708366400",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "street_lamp"
    ],
    "depth_offset": 0.0385437011718750
  }
},
{
  "cid": 3,
  "aoid": "852632353511957:1747354057708376400",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -0.2653045654296875,
        "Y": -0.0610275268554688
      },
      {
        "X": 0.0206146240234375,
        "Y": -0.2234458923339844
      },
      {
        "X": 0.2891464233398438,
        "Y": -0.0680618286132812
      },
      {
        "X": 0.0129928588867188,
        "Y": 0.1095085144042969
      }
    ]
  }
}
