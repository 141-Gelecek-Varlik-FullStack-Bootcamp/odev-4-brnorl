using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using odev3.DB.Models;
using odev3.Models.Pagination;
using odev3.Models.Product;

namespace odev3.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;

        public ProductService(IMapper _mapper)
        {
            mapper = _mapper;
        }

        public bool Insert(odev3.DB.Models.Product newProduct)
        {
            bool result = false;
            using (var srv = new ProjeContext())
            {
                srv.Products.Add(newProduct);
                newProduct.IsActive = true;
                newProduct.IsDeleted = false;
                srv.SaveChanges();
                result = true;
            }
            return result;
        }

        public ProductListModel<ProductViewModel> Get()
        {
            var result = new ProductListModel<ProductViewModel>();
            using (var srv = new ProjeContext())
            {
                var data = srv.Products.Where(
                    p => p.IsActive && !p.IsDeleted).OrderBy(p => p.Id);
                result.productList = mapper.Map<List<ProductViewModel>>(data);
            }
            return result;
        }
        public bool Update(UpdateProductModel updatedProduct, int id)
        {
            bool result = false;
            using (var srv = new ProjeContext())
            {
                var data = srv.Products.SingleOrDefault(p => p.Id == id);
                if (data is null)
                {
                    return result;
                }
                data.Name = updatedProduct.Name != default ? updatedProduct.Name : data.Name;
                data.DisplayName = updatedProduct.DisplayName != default ? updatedProduct.DisplayName : data.DisplayName;
                data.Price = updatedProduct.Price != default ? updatedProduct.Price : data.Price;

                srv.SaveChanges();
                result = true;
            }
            return result;
        }
        public bool Delete(int id)
        {
            bool result = false;
            using (var srv = new ProjeContext())
            {
                var data = srv.Products.SingleOrDefault(p => p.Id == id);
                if (data is null)
                {
                    return result;
                }
                //DB'den silinmez, silindi olarak işaretlenir
                srv.Products.SingleOrDefault(p => p.Id == id).IsDeleted = true;
                srv.Products.SingleOrDefault(p => p.Id == id).IsActive = false;
                srv.SaveChanges();
                result = true;
            }
            return result;
        }

        public PagedResponse<List<ProductViewModel>> GetPaged(int pageNumber, int pageSize)
        {//ilk olarak ürün listesi alınır.
            var products = Get().productList;
            //ürün listesi sayfalanıp pagedData haline getirilir
            var pagedData = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            var response = new PagedResponse<List<ProductViewModel>>(pagedData, pageNumber, pageSize);
            //response içine kullanıcının isteğine göre toplam ürün sayısı ve toplam sayfa işlenir
            response.TotalRecords = products.Count<ProductViewModel>();
            response.TotalPages = response.TotalRecords / pageSize;
            return response;
        }
    }
}