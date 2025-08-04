13
287762808833
473089675180945 1744302645252957900
{
  "name": "ShotgunExplosionVFX",
  "local_enabled": true,
  "local_position": {
    "X": 5.6924319267272949,
    "Y": -0.8904993534088135
  },
  "local_rotation": 0,
  "local_scale": {
    "X": 1.5000000000000000,
    "Y": 1.5000000000000000
  },
  "spawn_as_networked_entity": true,
  "network_position": true
},
{
  "cid": 1,
  "aoid": "473143790271407:1744302659521466100",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "anims/fireball/BAT003_fireball.spine",
    "ordered_skins": [
      "default"
    ]
  }
},
{
  "cid": 2,
  "aoid": "474939298577713:1744303132942544600",
  "component_type": "Mono_Component",
  "mono_component_type": "SimpleVFX",
  "data": {
    "Lifetime": 1.5000000000000000,
    "Skeleton": "473143790271407:1744302659521466100",
    "InitialAnimationName": "explode"
  }
}
