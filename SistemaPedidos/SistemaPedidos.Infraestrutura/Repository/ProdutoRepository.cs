using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaPedidos.Infraestrutura.Repository
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private const string FILE_NAME = "produtos.json";

        public ProdutoRepository() : base(FILE_NAME)
        {
        }

        public void Create(Produto produto)
        {
            var list = Load();
            list.Add(produto);
            Save(list);
        }

        public void Update(Guid id, Produto produto)
        {
            var list = Load();

            var index = list.FindIndex(p => p.Id == id);
            if(index >= 0)
            {
                list[index] = produto;
                Save(list);
            }
        }

        public void Delete(Guid id)
        {
            var list = Load();
            list.RemoveAll(p => p.Id == id);
            Save(list);
        }

        public Produto GetById(Guid id)
        {
            return Load().Find(p => p.Id == id);
        }

        public List<Produto> Search(string nome, string codigo, decimal valorInicial, decimal valorFinal)
        {
            var query = Load().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(p => p.Nome.Contains(nome));

            if (!string.IsNullOrWhiteSpace(codigo))
                query = query.Where(p => p.Codigo.Contains(codigo));

            if (valorInicial > 0)
                query = query.Where(p => p.Valor >= valorInicial);

            if (valorFinal > 0)
                query = query.Where(p => p.Valor <= valorFinal);

            return query.ToList();
        }

        
    }
}
