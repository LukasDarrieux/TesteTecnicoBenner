using SistemaPedidos.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SistemaPedidos.Domain.Interfaces
{
    public interface IPessoaRepository : IDisposable
    {
        void Create(Pessoa pessoa);
        void Update(Guid id, Pessoa pessoa);
        void Delete(Guid id);
        Pessoa GetById(Guid id);
        List<Pessoa> Search(string nome, string cpf);
    }
}
