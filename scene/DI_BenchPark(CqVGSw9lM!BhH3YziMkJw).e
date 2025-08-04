13
5905580032007
749117773175116 1749610577416831600
{
  "name": "DI_BenchPark",
  "local_enabled": true,
  "local_position": {
    "X": -24.5858383178710938,
    "Y": -26.0528450012207031
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "749117771948240:1749610577416545800",
  "next_sibling": "749117773787651:1749610577416974400",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_BenchPark.prefab"
},
{
  "cid": 1,
  "aoid": "749117773272812:1749610577416854400",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "bench_iron",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "749117773288378:1749610577416858000",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "bench_park"
    ],
    "depth_offset": 0.5632076263427734
  }
},
{
  "cid": 3,
  "aoid": "749117773308115:1749610577416862600",
  "component_type": "Mono_Component",
  "mono_component_type": "Chair",
  "data": {
    "Offset": {
      "Y": 0.3000000119209290
    }
  }
},
{
  "cid": 4,
  "aoid": "749117773322649:1749610577416866000",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
