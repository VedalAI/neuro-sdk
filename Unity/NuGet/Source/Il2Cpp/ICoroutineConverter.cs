namespace NeuroSdk.Il2Cpp
{
    public interface ICoroutineConverter
    {
        bool CanBeUsed { get; }
        object ConvertToIl2Cpp(System.Collections.IEnumerator enumerator);
    }
}
