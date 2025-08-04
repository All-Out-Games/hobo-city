using AO;

namespace ReusableWeapons
{
    /// <summary>
    /// Spawns a prefab at a given point
    /// Useful for spawning VFX but not technically limited to VFX
    /// Used throughout the weapons systems for things like explosions, hit effects, etc
    /// </summary>
    public class VFXManager : Singleton<_VFXManager> { }
    public partial class _VFXManager : Component
    {
        public void TrySpawnVFX(Prefab vfx, Vector2 position, float scale = 1.0f, Entity parent = null)
        {
            var instance = Entity.Instantiate(vfx, e => e.SetParent(parent, false));
            instance.Position = position;
            instance.Scale = Vector2.One * scale;
        }

        [ClientRpc]
        public void TrySpawnVFXByName(string vfxName, Vector2 position, float scale = 1.0f, Entity parent = null)
        {
            var vfx = Assets.GetAsset<Prefab>(vfxName);
            if (vfx != null)
            {
                TrySpawnVFX(vfx, position, scale, parent);
            }
        }
    }
}
