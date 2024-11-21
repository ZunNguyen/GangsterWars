using UnityEngine;

namespace Sources.Extension
{
    public static class LeaderKey
    {
        public static string GunId_Default = "gun-01";
        public static string GunId_02 = "gun-02";
        public static string GunId_03 = "gun-03";
        public static string GunId_04 = "gun-04";
        public static string GunId_05 = "gun-05";

        public static int Quality_Bullet_Default = 50;

        public static string ANIMATIONKEY_RELOADING = "Reloading";
    }

    public static class BomberKey
    {
        public static string BomberId_Default = "bom-01";

        public static int Quality_Bom_Default = 10;

        public static string AnimationKey_Reloading = "Reloading";
        public static string AnimationKey_Throwing = "Throwing";
    }

    public static class SniperKey
    {
        public static string SNIPERID_DEFAULT = "sniper-01";

        public static int QUALITY_SNIPER_DEFAULT = 50;

        public static string ANIMATIONKEY_RELOADING = "Reloading";
        public static string ANIMATIONKEY_SHOOTING = "Shooting";
    }

    public class LevelUpgradeKey
    {
        public static string LEVELUPGRADE_DEFAULT = "level-01";
    }

    public static class ShieldKey
    {
        public static string ShieldId_Default = "shield-01";
    }

    public static class JourneyKey
    {
        public static string JourneyItemViewDefault = "jourItem-01";
        public static string WaveDefault = "wave-01";
        public static string BASE_JOURNEY_ITEM = "jourItem";
        public static string BASE_LINK_HORIZOTAL_ITEM = "horizontal";
        public static string BASE_LINK_VERTICAL_ITEM = "vertical";
    }

    public static class CollisionTagKey
    {
        public static string SHIELD_USER = "ShieldUser";
        public static string BULLET_LEADER = "BulletLeader";
        public static string BULLET_SNIPER = "BulletSniper";
        public static string BOM_BOMBER = "BomBomber";
        public static string ENEMY_HEAD = "EnemyHead";
        public static string ENEMY_BODY = "EnemyBody";
    }

    public static class WaveKey
    {
        public static string WAVE_ID_FIRST_EPISODE_1 = "wave-01";
        public static string WAVE_ID_FIRST_EPISODE_2 = "wave-11";
        public static string WAVE_ID_FIRST_EPISODE_3 = "wave-21";
        public static string WAVE_ID_FIRST_EPISODE_4 = "wave-31";
        public static string WAVE_ID_MAX = "wave-40";
    }
}