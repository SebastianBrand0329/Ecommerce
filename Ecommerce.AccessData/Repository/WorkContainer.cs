using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using System.Drawing.Printing;

namespace Ecommerce.AccessData.Repository
{
    public class WorkContainer : IWorkContainer
    {
        private readonly ApplicationDbContext _context;

        public IWarehouse warehouse { get; private set; }

        public ICategory category { get; private set; }

        public ICompany company { get; private set; }   

        public IInventory inventory { get; private set; }  
        
        public IInventoryDetails inventoryDetails { get; private set; }

        public IKardexInventory kardexInventory { get; private set; }    

        public IModel model { get; private set; } 
        
        public IProduct product { get; private set; }

        public IProductWarehouse productWarehouse { get; private set; } 

        public IOrder order { get; private set; }   

        public IOrderDetail orderDetail { get; private set; }

        public IShoppinCar shoppinCar { get; private set; }

        public IUser user { get; private set; } 

        public WorkContainer(ApplicationDbContext context)
        {
            _context = context;
            warehouse = new WarehouseRepository(_context);
            category = new CategoryRepository(_context);    
            model = new ModelRepository(_context);  
            product = new ProductRepository(_context);  
            user = new UserRepository(_context);
            productWarehouse = new ProductWarehouseRepository(_context);
            inventory = new InventoryRepository(_context);
            inventoryDetails = new InventoryDetailsRepository(_context);
            kardexInventory = new KardexInventoryRepository(_context);
            company = new CompanyRepository(_context);
            shoppinCar = new ShoppingCarRepository(_context);
            order = new OrderRepository(_context);
            orderDetail = new OrderDetailRepository(_context);  
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Saved()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
