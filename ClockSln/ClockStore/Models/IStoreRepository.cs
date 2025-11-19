namespace ClockStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Clock> Clocks { get; }
    }
}
