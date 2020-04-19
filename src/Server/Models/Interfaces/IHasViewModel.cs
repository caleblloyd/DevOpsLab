namespace DevOpsLab.Server.Models.Interfaces
{
    public interface IHasViewModel<out T>
    {
        public T ViewModel { get; }
    }
}
