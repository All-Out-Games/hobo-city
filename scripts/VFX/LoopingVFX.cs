using AO;

namespace ReusableWeapons
{
    public class LoopingVFX : SimpleVFX
    {
        [Serialized] public string LoopAnimationName;
        [Serialized] public string OutroAnimationName;
        [Serialized] public float OutroAnimationTime = 0.0f;

        private bool HasStartedOutro = false;

        public override void SetupSkeleton()
        {
            base.SetupSkeleton();

            var sm = Skeleton.SpineInstance.StateMachine;
            var baseLayer = sm.TryGetLayerByName("main");

            var introState = baseLayer.TryGetStateByName(InitialAnimationName);
            var loopState = baseLayer.CreateState(LoopAnimationName, 0, true);
            baseLayer.CreateTransition(introState, loopState, true);

            if (!string.IsNullOrEmpty(OutroAnimationName))
            {
                var outroState = baseLayer.CreateState(OutroAnimationName, 0, false);
                baseLayer.CreateTransition(loopState, outroState, false).CreateTriggerCondition(sm.CreateVariable("outro", StateMachineVariableKind.TRIGGER));
            }
        }

        public override void Update()
        {
            base.Update();

            if (!string.IsNullOrEmpty(OutroAnimationName) && Util.OneTime((Lifetime - LifetimeSoFar) <= OutroAnimationTime, ref HasStartedOutro))
            {
                Skeleton.SpineInstance.StateMachine.SetTrigger("outro");
            }
        }
    }
}