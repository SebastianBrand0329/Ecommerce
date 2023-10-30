namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IWorkContainer : IDisposable
    {
        IWarehouse warehouse { get; }

        ICategory category { get; }

        ICompany company { get; }   

        IInventory inventory { get; }

        IInventoryDetails inventoryDetails { get; }

        IKardexInventory kardexInventory { get; }

        IModel  model { get; }  

        IProduct product { get; }

        IProductWarehouse productWarehouse { get; }

        IOrder order { get; }

        IOrderDetail orderDetail { get; }

        IShoppinCar shoppinCar { get; } 

        IUser user { get; }

        Task Saved();
    }
}
