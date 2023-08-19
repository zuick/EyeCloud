namespace Game.Core
{
    public interface IAbility
    {
        void Apply(Entity owner, object data);
        float Duration { get; }
        string Name { get; }
    }
}