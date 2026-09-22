# Remove unused weapon merge

Removed `anims/reusable-weapons/player/player.merged_spine_rig`. The published source version `6ab20436236e3076fa1ad9c0` does not reference its output in scripts, scenes, serialized data or decoded bundled assets. A commented example in NpcStoreTrigger.cs names a different, obsolete recipe; it is not an active dependency. The required `rigs/merged-rig/merged-rig.merged_spine_rig` and all its data remain unchanged.

Game: `688d1c6023e5aae65f04fcd2` (Gun Game).
Published source SHA-256: `965899f5430f187e054031d25b12518d40bf4bce93d6a4af2b78e72dfd927ab9`.
Prepared candidate SHA-256: `c7a621b03488c5139103b85b3736082dae5ec3101e169c18cd88446c6c65ae09`.

The candidate is derived from the exact published source archive, preserving all remaining scripts, scenes and cooked assets byte-for-byte. Only the unused recipe and corresponding manifest/bundle records are removed. Cloud compilation and publication evidence is maintained in the engine repository's rig-startup audit. The source archive URL predates the active P48 compiled version; compilation must retain P48. This commit alone does not establish deployment.
