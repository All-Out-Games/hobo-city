13
7237019893762
38718343452514 1747342891291645400
{
  "name": "DI_Sandcastle",
  "local_enabled": true,
  "local_position": {
    "X": 40.9234085083007812,
    "Y": 2.0648727416992188
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
  "aoid": "38718343906906:1747342891291771100",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 80,
    "skin": "Sandcastle",
    "RespawnTime": 5,
    "CashReward": 5
  }
},
{
  "cid": 2,
  "aoid": "38718343980058:1747342891291791200",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "rigs/destructible-item-2/016ARP_Destructible_Items_2.spine",
    "ordered_skins": [
      "sandcastle"
    ],
    "depth_offset": 0.8102874755859375
  }
},
{
  "cid": 3,
  "aoid": "38718344065738:1747342891291815000",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "make_navmesh_loop": true,
    "flip_navmesh_loop": true,
    "points": [
      {
        "X": -0.9625015854835510,
        "Y": 0.0206680316478014
      },
      {
        "X": 0.9135971069335938,
        "Y": -0.0027313232421875
      },
      {
        "X": 0.9525681138038635,
        "Y": 0.0258102435618639
      },
      {
        "X": 0.8357773423194885,
        "Y": 0.6800003647804260
      },
      {
        "X": -0.8405990600585938,
        "Y": 0.6849761009216309
      }
    ]
  }
}
