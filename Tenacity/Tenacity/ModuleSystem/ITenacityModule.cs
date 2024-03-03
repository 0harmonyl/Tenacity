namespace Tenacity.ModuleSystem
{
    public interface ITenacityModule
    {
        public string Name { get; }
        public string Description { get; }
        public string Tab { get; }
        public void OnEnable();
        public void OnDisable();
    }
}