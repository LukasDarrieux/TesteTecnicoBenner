using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SistemaPedidos.Infraestrutura.Repository
{
    public class PessoaRepository : BaseRepository<Pessoa>, IPessoaRepository
    {
        private const string FILE_NAME = "pessoas.json";

        public PessoaRepository() : base(FILE_NAME)
        {
        }

        public void Create(Pessoa pessoa)
        {
            var list = Load();
            list.Add(pessoa);
            Save(list);
        }

        public void Update(Guid id, Pessoa pessoa)
        {
            var list = Load();

            var index = list.FindIndex(x => x.Id == id);
            if (index >= 0)
            {
                list[index] = pessoa;
                Save(list);
            }
        }

        public void Delete(Guid id)
        {
            var list = Load();
            list.RemoveAll(x => x.Id == id);
            Save(list);
        }

        public Pessoa GetById(Guid id)
        {
            return Load().Find(x => x.Id == id);
        }

        public List<Pessoa> Search(string nome, string cpf)
        {
            var query = Load().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(p => p.Nome.Contains(nome));

            if (!string.IsNullOrWhiteSpace(cpf))
                query = query.Where(p => p.CPF == cpf);

            return query.ToList();
    
        }
    }
}