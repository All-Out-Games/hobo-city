13
12133282611201
16211622430756 1747407286656070200
{
  "name": "DI_vault_door",
  "local_enabled": true,
  "local_position": {
    "X": 42.8542022705078125,
    "Y": 4.6445083618164062
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1,
    "Y": 1
  },
  "spawn_as_networked_entity": true
},
{
  "cid": 1,
  "aoid": "16211622887002:1747407286656196600",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "vault_door",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "16211622976714:1747407286656221200",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "vault_door"
    ],
    "depth_offset": 0.0632934570312500
  }
},
{
  "cid": 4,
  "aoid": "246044552567263:1748535876224396800",
  "component_type": "Internal_Component",
  "internal_component_type": "Box_Collider",
  "data": {
    "size": {
      "X": 2.7198181152343750,
      "Y": 0.3249382972717285
    },
    "offset": {
      "X": -0.0486755371093750,
      "Y": 0.2012202739715576
    }
  }
}
