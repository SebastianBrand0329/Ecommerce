namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IWorkContainer : IDisposable
    {
        IWarehouse warehouse { get; }

        ICategory category { get; }

        Task Saved();
    }
}
