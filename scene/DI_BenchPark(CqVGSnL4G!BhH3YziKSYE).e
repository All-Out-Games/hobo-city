13
3350074490887
749117770612230 1749610577416234500
{
  "name": "DI_BenchPark",
  "local_enabled": true,
  "local_position": {
    "X": -16.0612411499023438,
    "Y": -27.0316543579101562
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "previous_sibling": "749117769939194:1749610577416077700",
  "next_sibling": "749117771302208:1749610577416395200",
  "parent": "572739174724627:1745626489395237700",
  "spawn_as_networked_entity": true,
  "linked_prefab": "DI_BenchPark.prefab"
},
{
  "cid": 1,
  "aoid": "749117770718526:1749610577416259200",
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
  "aoid": "749117770736328:1749610577416263400",
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
  "aoid": "749117770756065:1749610577416268000",
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
  "aoid": "749117770771201:1749610577416271500",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {

  }
}
