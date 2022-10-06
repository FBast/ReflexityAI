namespace Examples.TankArena.Scripts.SOEvents {
    public interface IGameEventListener<T> {
        void OnEventRaised(T item);
    }
}