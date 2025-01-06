using SV21T1020620.DomainModels;

namespace SV21T1020620.DataLayers
{
    public interface ICartDAL
    {
        public int Add(Cart data);
        public int AddCartDetail(Cartdetail data);
        public int Delete(int customerID);
        public bool Update(Cart data);
        public int Count(int customerID);
        public Cart Get(int customerID);
        public List<ViewCart> GetDetailList(int customerID);
        public Cartdetail GetDetail(int cartID, int productID);
        bool SaveDetail(int cartID, int productID, int quantity);
        bool DeleteDetail(int cartDetailID);
        bool SaveCart(int customerID, int sum);
        public Cartdetail? checkProductExists(int cartID, int productID);
        public List<ViewCart> GetViewCarts(int customerID);
    }
}
