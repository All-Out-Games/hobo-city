using AO;

public class Npc : Component
{
    [Serialized] public int CrewschiaIndex = 0;
    public Spine_Animator SpineAnimator;
    ulong rngSeed;

    public string[] RandomOutfits = {
        "full_character/admiral_full",
        "full_character/alien",
        "full_character/android",
        "full_character/anger_full",
        "full_character/ant_full",
        "full_character/anxiety_full",
        "full_character/archangel_gold_full",
        "full_character/aristocrat_full",
        "full_character/artist_full",
        "full_character/astronaut_full",
        "full_character/baby1_full",
        "full_character/baby2_full",
        "full_character/baby3_full",
        "full_character/ballerina_full",
        "full_character/bandit_full",
        "full_character/bat_full",
        "full_character/batdude_full",
        "full_character/beekeeper_beehive_full",
        "full_character/beekeeper_full",
        "full_character/beetle_full",
        "full_character/biffie_full",
        "full_character/bride_full",
        "full_character/Bronze_goose_full",
        "full_character/cardboard_full",
        "full_character/chicken_full",
        "full_character/clown_full",
        "full_character/cool_dad_full",
        "full_character/cop_full",
        "full_character/cosmos_lord_alt_full",
        "full_character/cosmos_lord_full",
        "full_character/cowboy_full",
        "full_character/creepy_scarecrow_full",
        "full_character/cupid_full",
        "full_character/deep_diver_full",
        "full_character/detective_full",
        "full_character/diamond_armor_full",
        "full_character/discord_full",
        "full_character/dog_retriever_full",
        "full_character/dogcop_full",
        "full_character/dragon_full",
        "full_character/dwarf_full",
        "full_character/elements_master_full",
        "full_character/engineer_full",
        "full_character/evil_sorcerer_full",
        "full_character/explorer_full",
        "full_character/exterminator_full",
        "full_character/fairy_princess_full",
        "full_character/fem_full",
        "full_character/firefighter_full",
        "full_character/fish_full",
        "full_character/fisherman_full",
        "full_character/flower_maiden_full",
        "full_character/fungus_full",
        "full_character/gamer_full",
        "full_character/garden_gnome_full",
        "full_character/gargoyle_full",
        "full_character/garryblox_full",
        "full_character/gladiator_full",
        "full_character/gold_poop_full",
        "full_character/golden_goose_full",
        "full_character/golden_suit_full",
        "full_character/gremlin_full",
        "full_character/grizzled_prisoner_full",
        "full_character/groom_full",
        "full_character/harlequinn_full",
    };

    public override void Awake()
    {
        SpineAnimator = GetComponent<Spine_Animator>();
        SpineAnimator.Awaken();
        SpineAnimator.SetCrewchsia(CrewschiaIndex);
        SpineAnimator.SpineInstance.SetAnimation("Idle", true);

        rngSeed = RNG.Seed(Entity.NetworkId);
    }

    public void SetRandomOutfit()
    {
        if (RandomOutfits.Length > 0)
        {
            int randomIndex = RNG.RangeInt(ref rngSeed, 0, RandomOutfits.Length - 1);
            string outfit = RandomOutfits[randomIndex];

            if (SpineAnimator != null && SpineAnimator.SpineInstance != null)
            {
                SpineAnimator.SpineInstance.DisableAllSkins();
                SpineAnimator.SpineInstance.EnableSkin("base/crewchsia");
                SpineAnimator.SpineInstance.EnableSkin(outfit);
                SpineAnimator.SpineInstance.RefreshSkins();
            }
        }
    }

    public void DoWelcomeAnimation()
    {
        SpineAnimator.SpineInstance.SetAnimation("Emote/Wave", false);
    }
}