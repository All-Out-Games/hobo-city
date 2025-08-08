13
8057358647297
3312129696873691 1749149796410163900
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
  "next_sibling": "3376855989105055:1754686763391309400",
  "parent": "3377173085251582:1754686837273568900",
  "spawn_as_networked_entity": true,
  "linked_prefab": "Door_Portal.prefab"
},
{
  "cid": 1,
  "aoid": "3312183595629343:1749149809267304100",
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
  "aoid": "3376472097625192:1754686673945978200",
  "component_type": "Mono_Component",
  "mono_component_type": "GenericReturnTeleporter",
  "data": {
    "ReturnToRoom": 11
  }
},
{
  "cid": 3,
  "aoid": "3376754665085081:1754686739783178100",
  "component_type": "Internal_Component",
  "internal_component_type": "Interactable",
  "data": {
    "text": "Start Playing",
    "radius": 3.4000000953674316,
    "required_hold_time": 0.4000000059604645
  }
}
