using SistemaPedidos.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SistemaPedidos.Domain.Interfaces
{
    public interface IProdutoRepository : IDisposable
    {
        void Create(Produto produto);
        void Update(Guid id, Produto produto);
        void Delete(Guid id);
        Produto GetById(Guid id);
        List<Produto> Search(string nome, string codigo, decimal valorInicial, decimal valorFinal);
    }
}
