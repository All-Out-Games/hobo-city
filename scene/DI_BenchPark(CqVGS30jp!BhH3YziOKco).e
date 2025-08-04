13
5918464933895
749117774973161 1749610577417250600
{
  "name": "DI_BenchPark",
  "local_enabled": true,
  "local_position": {
    "X": 17.1564254760742188,
    "Y": -28.1146507263183594
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "749117773787651:1749610577416974400",
  "next_sibling": "1121578651576013:1749317273643621000",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_BenchPark.prefab"
},
{
  "cid": 1,
  "aoid": "749117775075845:1749610577417274500",
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
  "aoid": "749117775092056:1749610577417278200",
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
  "aoid": "749117775110675:1749610577417282600",
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
  "aoid": "749117775124435:1749610577417285800",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
