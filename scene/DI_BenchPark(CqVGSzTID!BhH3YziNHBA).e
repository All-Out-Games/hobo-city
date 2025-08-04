13
5909874999303
749117773787651 1749610577416974400
{
  "name": "DI_BenchPark",
  "local_enabled": true,
  "local_position": {
    "X": -32.4095687866210938,
    "Y": -48.8255691528320312
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "749117773175116:1749610577416831600",
  "next_sibling": "749117774973161:1749610577417250600",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_BenchPark.prefab"
},
{
  "cid": 1,
  "aoid": "749117773886594:1749610577416997400",
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
  "aoid": "749117773904095:1749610577417001400",
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
  "aoid": "749117773923058:1749610577417005900",
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
  "aoid": "749117773938968:1749610577417009600",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
