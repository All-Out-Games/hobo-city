13
128849018881
445824869075156 1745344685956531200
{
  "name": "Car_1",
  "local_enabled": true,
  "local_position": {

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
  "aoid": "445824869308004:1745344685956586500",
  "component_type": "Mono_Component",
  "mono_component_type": "Destructable",
  "data": {
    "MaxHealth": 100,
    "skin": "car_1",
    "RespawnTime": 30,
    "CashReward": 15
  }
},
{
  "cid": 2,
  "aoid": "445824869366174:1745344685956600300",
  "component_type": "Internal_Component",
  "internal_component_type": "Spine_Animator",
  "data": {
    "skeleton_data_asset": "destructables/destructables-spine/016ARP_ Destructible_Items.spine",
    "ordered_skins": [
      "car_1"
    ],
    "depth_offset": 0.2500000000000000
  }
},
{
  "cid": 3,
  "aoid": "445824869442614:1745344685956618500",
  "component_type": "Internal_Component",
  "internal_component_type": "Polygon_Collider",
  "data": {
    "points": [
      {
        "X": -1.1870727539062500,
        "Y": -0.0235559344291687
      },
      {
        "X": 1.1870803833007812,
        "Y": -0.0235558748245239
      },
      {
        "X": 1.2874526977539062,
        "Y": 0.7627333402633667
      },
      {
        "X": -1.2327041625976562,
        "Y": 0.7536075115203857
      }
    ]
  }
}
