14
16230681411585
3681597969
{
  "name": "NPC_GunDealer",
  "local_enabled": true,
  "local_position": {
    "X": 5.9250564575195312,
    "Y": 3.5947484970092773
  },
  "local_rotation": 0,
  "local_scale": {
    "X": -1,
    "Y": 1
  },
  "next_sibling": 111588922,
  "parent": 1760120498,
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": 495543384,
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
  "aoid": 3746915795,
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {
    "text": "'Sellin? I'm Buy'in",
    "radius": 3.5000000000000000
  }
},
{
  "cid": 4,
  "aoid": 3452700778,
  "component_type": "Mono_Component",
  "mono_component_type": "NpcTrigger",
  "data": {
    "Interactable": 3746915795,
    "shopType": 4
  }
}
