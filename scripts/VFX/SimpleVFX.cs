using AO;

namespace ReusableWeapons
{
    public class SimpleVFX : Component
    {
        [Serialized] public float Lifetime = 2.0f; // Set to 0 to make it not despawn, requires manual cleanup in this case

        [Serialized] public Spine_Animator Skeleton;
        [Serialized] public string InitialAnimationName;

        protected float LifetimeSoFar = 0;

        public override void Awake()
        {
            base.Awake();

            SetupSkeleton();
        }

        public virtual void SetupSkeleton()
        {
            Skeleton.Awaken();
            var sm = StateMachine.Make();
            Skeleton.SpineInstance.SetStateMachine(sm, Entity);

            var baseLayer = sm.CreateLayer("main");

            var idleState = baseLayer.CreateState(InitialAnimationName, 0, false);
            baseLayer.InitialState = idleState;
        }

        public override void Update()
        {
            base.Update();

            if (Lifetime > 0.0f)
            {
                LifetimeSoFar += Time.DeltaTime;

                if (LifetimeSoFar >= Lifetime)
                {
                    Entity.Destroy();
                }
            }
        }
    }
}