namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IWorkContainer : IDisposable
    {
        IWarehouse warehouse { get; }

        ICategory category { get; }

        IModel  model { get; }  

        Task Saved();
    }
}
