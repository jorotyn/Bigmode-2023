public abstract class StateBase : UnityHFSM.StateBase
{
    protected StateBase(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }
}
