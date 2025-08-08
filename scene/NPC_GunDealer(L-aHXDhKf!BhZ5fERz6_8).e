13
34359738369
3375096283402911 1754686353386221500
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
  "previous_sibling": "358517446368064:1746647064116659500",
  "parent": "349415208196092:1746644664294138800",
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": "3375096283582092:1754686353386263000",
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
  "aoid": "3375096283666458:1754686353386282700",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {
    "text": "Sell Guns",
    "radius": 3.5000000000000000
  }
},
{
  "cid": 4,
  "aoid": "3375096283706491:1754686353386292000",
  "component_type": "Mono_Component",
  "mono_component_type": "NpcTrigger",
  "data": {
    "Interactable": "3375096283666458:1754686353386282700",
    "shopType": 4
  }
}
