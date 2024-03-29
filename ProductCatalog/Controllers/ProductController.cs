using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProductController(ProductRepository repository)
        {
            _repository = repository;
        }

        [Route("v1/products")]
        [HttpGet]
        public IEnumerable<ListProductViewModel> Get()
        {
            return _repository.Get();
        }

        [Route("v1/products/{id}")]
        [HttpGet]
        public Product Get(int id)
        {
            return _repository.Get(id);
        }

        [Route("v1/products")]
        [HttpPost]
        public ResultViewModel Post([FromBody]EditProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
               return new ResultViewModel
               {
                   Success = false,
                   Message = "Não foi possível cadastrar o produto",
                   Data = model.Notifications
               };

            var product = new Product();
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.CreateDate = DateTime.Now; // Nunca receber esta informação
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdate = DateTime.Now; // Nunca receber esta informação
            product.Price = model.Price;
            product.Quantity = model.Quantity;

            // Salvar
            _repository.Save(product);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso.",
                Data = product
            };
        }

        [Route("v1/products")]
        [HttpPut]
        public ResultViewModel Put([FromBody]EditProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
               return new ResultViewModel
               {
                   Success = false,
                   Message = "Não foi possível atualizar o produto",
                   Data = model.Notifications
               };

            var product = _repository.Get(model.Id); 
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdate = DateTime.Now; // Nunca receber esta informação
            product.Price = model.Price;
            product.Quantity = model.Quantity;

            // Salvar
            _repository.Update(product);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto alterado com sucesso.",
                Data = product
            };
        }
    }
}