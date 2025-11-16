using AO;
using System;

public class EffectDodgeRoll : MyEffect
{
    public override bool IsActiveEffect => true;

    public override bool BlockAbilityActivation => true;
    public override bool IsValidTarget => false;

    public override float DefaultDuration => 0.467f;

    float _invulnerableUntil = -1f;

    float GetMag(float x, float y)
    {
        float sum = (x * x) + (y * y);
        sum = (float)Math.Sqrt(sum);
        return sum;
    }

    public override void OnEffectStart(bool isDropIn)
    {
        SFX.Play(Assets.GetAsset<AudioAsset>("sfx/dodge-sfx.wav"), new() { Positional = true, Position = Entity.Position, Volume = 0.5f, EntityToFollow = Player.Entity, SpeedPerturb = 0.25f });
        Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger("dodge_roll");

        float mag = 42.5f;

        if (!isDropIn)
        {
            if (GetMag(Player.Agent.Velocity.X, Player.Agent.Velocity.Y) >= 0.5f)
            {
                Player.AddDash(Player.Agent.Velocity * mag, 0.467f);
            }
            else
            {
                Player.AddDash(new Vector2(Player.GetFacingDirection() ? 1 : -1, 0) * (mag * 10), 0.467f);
            }
        }

        // Grant brief invulnerability frames on dodge start (server authoritative)
        if (Network.IsServer && Player.HealthManager != null)
        {
            Player.HealthManager.IsInvulnerable = true;
            _invulnerableUntil = Time.TimeSinceStartup + 0.45f;
        }
    }

    public override void OnEffectUpdate()
    {
        // End i-frames after the configured window
        if (Network.IsServer && Player.HealthManager != null && Player.HealthManager.IsInvulnerable)
        {
            if (_invulnerableUntil > 0 && Time.TimeSinceStartup >= _invulnerableUntil)
            {
                Player.HealthManager.IsInvulnerable = false;
                _invulnerableUntil = -1f;
            }
        }
    }

    public override void OnEffectEnd(bool interrupt)
    {
        // Ensure invulnerability is cleared when the effect ends
        if (Network.IsServer && Player.HealthManager != null)
        {
            Player.HealthManager.IsInvulnerable = false;
            _invulnerableUntil = -1f;
        }
    }
}


