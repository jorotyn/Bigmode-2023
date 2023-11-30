public static class StateMachineExtensions
{
    public static void AddState<T>(this UnityHFSM.StateMachine stateMachine) where T : StateBase, new()
	    => stateMachine.AddState(typeof(T).Name, new T());
}
