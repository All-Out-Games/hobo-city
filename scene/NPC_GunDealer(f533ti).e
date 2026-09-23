14
34359738369
199093864
{
  "name": "NPC_GunDealer",
  "local_enabled": true,
  "local_position": {
    "X": 137.8079223632812500,
    "Y": 17.4025573730468750
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "previous_sibling": 1626675228,
  "parent": 1760120498,
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": 3125763107,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/NPC/016ARP_NPC.spine",
    "ordered_skins": [
      "base/gun_dealer"
    ],
    "initial_animation": "016ARP/Gun_Dealer_Idle",
    "loop_initial_animation": true
  }
},
{
  "cid": 3,
  "aoid": 171887034,
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {
    "text": "Sell Guns",
    "radius": 3.5000000000000000
  }
},
{
  "cid": 4,
  "aoid": 3441720229,
  "component_type": "Mono_Component",
  "mono_component_type": "NpcTrigger",
  "data": {
    "Interactable": 171887034,
    "shopType": 4
  }
}
