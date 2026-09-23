14
8057358647297
1770130835
{
  "name": "Door_Portal",
  "local_enabled": true,
  "local_position": {
    "X": -0.0675048828125000,
    "Y": -0.4500370025634766
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1.5000000000000000,
    "Y": 1.5000000000000000
  },
  "next_sibling": 1426283204,
  "parent": 766205571,
  "spawn_as_networked_entity": true,
  "linked_prefab": "Door_Portal.prefab"
},
{
  "cid": 1,
  "aoid": 3124409950,
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/door_portal/016ARP_Door_Portal.spine",
    "ordered_skins": [

    ],
    "depth_offset": 0.0437531471252441,
    "initial_animation": "016ARP/Idle",
    "loop_initial_animation": true
  }
},
{
  "cid": 2,
  "aoid": 1249911836,
  "component_type": "Mono_Component",
  "mono_component_type": "GenericReturnTeleporter",
  "data": {
    "ReturnToRoom": 11
  }
},
{
  "cid": 3,
  "aoid": 1244574265,
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {
    "text": "Start Playing",
    "radius": 3.4000000953674316,
    "required_hold_time": 0.4000000059604645
  }
}
