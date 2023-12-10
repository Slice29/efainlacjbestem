using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using ProdLib.General;
using ProdLib.SearchProducts;

//* Deprecated class. Used while testing. Use for reference to send equivalent
//* of GET request for all products with authorization

namespace UserApiService.MessagePublishers
{
    public class GetAllProductsPublisher
    {
        private readonly IRequestClient<PublishGetAllProducts> _requestClient;

        public GetAllProductsPublisher(IRequestClient<PublishGetAllProducts> requestClient)
        {
            _requestClient = requestClient;
        }
        public async Task<IEnumerable<Product>> GetAllProducts(string token)
        {
            var response = await _requestClient.GetResponse<ResponseGetAllProducts>(new
            {
                Token = token
            });
            var responseMessage = response.Message;
            IEnumerable<Product> products = response.Message.ProductList;
           return response.Message.ProductList;
        }
    }
}