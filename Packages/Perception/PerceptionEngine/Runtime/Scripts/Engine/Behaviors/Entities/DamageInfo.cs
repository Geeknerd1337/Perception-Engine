using UnityEngine;
namespace Perception.Engine
{
    public struct DamageInfo
    {
        public Entity Attacker;
        public Entity Weapon;
        public Vector3 Position;
        public Vector3 Force;
        public Rigidbody Body;
        public float Damage;
        public DamageFlags Flags;

        public static DamageInfo FromBullet(Vector3 hitPosition, Vector3 hitForce, float damage)
        {
            DamageInfo result = default(DamageInfo);
            result.Position = hitPosition;
            result.Force = hitForce;
            result.Damage = damage;
            result.Flags = DamageFlags.Bullet;
            return result;
        }

        public static DamageInfo FromMelee(Vector3 hitPosition, Vector3 hitForce, float damage)
        {
            DamageInfo result = default(DamageInfo);
            result.Position = hitPosition;
            result.Force = hitForce;
            result.Damage = damage;
            result.Flags = DamageFlags.Melee;
            return result;
        }

        public static DamageInfo Generic(float damage)
        {
            DamageInfo result = default(DamageInfo);
            result.Damage = damage;
            result.Flags = DamageFlags.Generic;
            return result;
        }

        public static DamageInfo Explosion(Vector3 position, Vector3 force, float damage)
        {
            DamageInfo result = default(DamageInfo);
            result.Position = position;
            result.Force = force;
            result.Damage = damage;
            result.Flags = DamageFlags.Blast;
            return result;
        }

    }

    public enum DamageFlags
    {
        Generic = 0,
        Bullet = 1,
        Blast = 2,
        Melee = 3,
    }
}
