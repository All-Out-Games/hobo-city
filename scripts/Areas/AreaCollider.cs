using AO;
using System;

// DO NOT CHANGE ORDER OR ADD TO FRONT
public enum LandmarkArea
{
    SoccerStadium,
    Beach,
    Park,
    Hospital,
    Bank,
    PoliceStation,
    Diner,
    ClothingStore,
    GunStore,
    GeneralStore,
    Club,
    Dealership,
    Airport,
    GarbageDump,
    ResidentialArea,
    CommercialArea,
    TedIsland,
    Ocean,
    None,
}

public class AreaCollider : Component
{
    [Serialized]
    public LandmarkArea AreaName;

    public override void Awake()
    {
        if (!Network.IsServer) return;

        Entity.GetComponent<Box_Collider>().OnCollisionEnter = (Entity other) =>
        {
            var player = other.GetComponent<MyPlayer>();
            if (!player.Alive()) return;

        };
    }

    public string GetDisplayName()
    {
        return GetDisplayName(AreaName);
    }

    public static string GetDisplayName(LandmarkArea area)
    {
        return area switch
        {
            LandmarkArea.SoccerStadium => "Soccer Stadium",
            LandmarkArea.Beach => "Beach",
            LandmarkArea.Park => "Park",
            LandmarkArea.Hospital => "Hospital",
            LandmarkArea.Bank => "Bank",
            LandmarkArea.PoliceStation => "Police Station",
            LandmarkArea.Diner => "Diner",
            LandmarkArea.ClothingStore => "Clothing Store",
            LandmarkArea.GunStore => "Gun Store",
            LandmarkArea.GeneralStore => "General Store",
            LandmarkArea.Club => "Club",
            LandmarkArea.Dealership => "Dealership",
            LandmarkArea.Airport => "Airport",
            LandmarkArea.GarbageDump => "Garbage Dump",
            LandmarkArea.ResidentialArea => "Residential Area",
            LandmarkArea.CommercialArea => "Commercial Area",
            LandmarkArea.TedIsland => "Ted Island",
            LandmarkArea.Ocean => "Ocean",
            _ => area.ToString()
        };
    }
}