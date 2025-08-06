13
16230681411585
144741219247130 1747800783255401900
{
  "name": "NPC_GunDealer",
  "local_enabled": true,
  "local_position": {
    "X": 6.0078506469726562,
    "Y": 3.5947484970092773
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "next_sibling": "505664764441075:1746715876644456000",
  "parent": "349415208196092:1746644664294138800",
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": "144741219452846:1747800783255450200",
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
  "aoid": "2285556299639224:1754432493564520200",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {
    "text": "'Sellin? I'm Buy'in",
    "radius": 2.7000000476837158
  }
},
{
  "cid": 4,
  "aoid": "2285599217916411:1754432503564371100",
  "component_type": "Mono_Component",
  "mono_component_type": "NpcTrigger",
  "data": {
    "Interactable": "2285556299639224:1754432493564520200",
    "shopType": 4
  }
}
