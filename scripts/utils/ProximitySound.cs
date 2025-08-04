using AO;

public class ProximitySound : Component
{
    [Serialized] public AudioAsset Sound;
    [Serialized] public float Volume = 0.5f;
    [Serialized] public float Range = 10f;
    [Serialized] public bool OnlyAtNight;
    [Serialized] public bool OnlyAtDay;
    [Serialized] public bool IsBgm;

    // Allows locally muting all background music (BGM) sounds for the current client only.
    public static bool GlobalBgmMuted = false;

    private ulong soundId;

    // Cached callback to unsubscribe on destroy
    private void OnDayNightSync(bool oldIsDay, bool isDay)
    {
        UpdateSoundPlayback(isDay);
    }

    public void UpdateSoundPlayback(bool isDay)
    {
        bool shouldPlay = true;

        // If this is marked as background music and the player has chosen to mute BGM, prevent playback.
        if (IsBgm && GlobalBgmMuted)
        {
            shouldPlay = false;
        }

        // Determine if the sound should be playing based on day/night flags
        if (OnlyAtNight && isDay) shouldPlay = false;
        if (OnlyAtDay && !isDay) shouldPlay = false;

        if (shouldPlay)
        {
            // Start the sound if it is not already playing
            if (soundId == 0)
            {
                soundId = SFX.Play(Sound, new SFX.PlaySoundDesc() { Volume = Volume, Position = Entity.Position, Positional = true, Loop = true, RangeMultiplier = Range });
            }
        }
        else
        {
            // Stop the sound if it's currently playing
            if (soundId != 0)
            {
                SFX.FadeOutAndStop(soundId, 2f);
                soundId = 0;
            }
        }
    }

    public override void Awake()
    {
        if (Network.IsServer)
        {
            return;
        }

        // Subscribe to day/night changes so we can react immediately
        GameManager.Instance.IsDay.OnSync += OnDayNightSync;

        // Initialise playback based on current day/night state
        UpdateSoundPlayback(GameManager.Instance.IsDay.Value);
    }

    public override void OnDestroy()
    {
        // Unsubscribe from the day/night sync event
        if (GameManager.Instance.Alive())
        {
            GameManager.Instance.IsDay.OnSync -= OnDayNightSync;
        }

        if (soundId != 0)
        {
            SFX.FadeOutAndStop(soundId, 0.45f);
        }
    }

    // Public helper to immediately stop any playing sound and ensure it does not restart.
    public void StopSound()
    {
        if (soundId != 0)
        {
            Log.Info("Stopping sound " + Entity.Name);
            SFX.FadeOutAndStop(soundId, 0.45f);
            soundId = 0;
        }

        // Disable further automatic playback attempts for this component instance.
        if (IsBgm)
        {
            GlobalBgmMuted = true;
        }
    }
}
