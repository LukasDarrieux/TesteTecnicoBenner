using SistemaPedidos.Aplicacao.CustomException;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace SistemaPedidos.Aplicacao.Handlers
{
    public class PessoaHandler : IDisposable
    {
        private readonly IPessoaRepository _pessoaRepository;
        public PessoaHandler(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        /// <summary>
        /// Pesquisa pessoas filtrando pelo nome e cpf, 
        /// ambos os filtros são opcionais, ou seja, 
        /// se não for passado nenhum filtro, 
        /// irá retornar todas as pessoas cadastradas.
        /// </summary>
        /// <param name="nome">Nome da pessoa</param>
        /// <param name="cpf">CPF da pessoa</param>
        /// <returns>Lista de pessoas que atendem aos filtros</returns>
        public List<Pessoa> Search(string nome, string cpf)
        {
            return _pessoaRepository.Search(nome, cpf);
        }

        /// <summary>
        /// Pesquisa pessoas 
        /// </summary>
        /// <returns>Lista de pessoas que atendem aos filtros</returns>
        public List<Pessoa> Search()
        {
            return _pessoaRepository.Search(string.Empty, string.Empty);
        }

        /// <summary>
        /// Obtém a instância de Pessoa correspondente ao identificador especificado.
        /// </summary>
        /// <param name="Id">O identificador exclusivo da Pessoa a ser recuperada.</param>
        /// <returns>A instância de Pessoa correspondente ao identificador fornecido.</returns>
        /// <exception cref="ValidationException">Lançada quando nenhuma Pessoa com o identificador especificado é encontrada.</exception>
        public Pessoa GetById(Guid Id)
        {
            var pessoa = _pessoaRepository.GetById(Id);
            if (pessoa == null) throw new ValidationException(new List<string>()
            {
                $"Pessoa com id {Id} não encontrada."
            });
            return pessoa;
        }

        /// <summary>
        /// Cadastra uma nova instância de Pessoa. Antes de criar a pessoa, é realizada a validação dos dados, e caso haja alguma inconsistência, uma ValidationException é lançada contendo os erros de validação encontrados.
        /// </summary>
        /// <param name="pessoa">A instância de Pessoa a ser criada.</param>
        /// <exception cref="ValidationException">Lançada quando os dados da Pessoa são inválidos.</exception>
        public void Create(Pessoa pessoa)
        {
            if (!pessoa.IsValid()) throw new ValidationException(pessoa.ValidationErrors);

            _pessoaRepository.Create(pessoa);
        }

        /// <summary>
        /// Atualiza uma instância existente de Pessoa com base no identificador fornecido. 
        /// Antes de realizar a atualização, é realizada a validação dos dados.
        /// </summary>
        /// <param name="Id">O identificador exclusivo da Pessoa a ser atualizada.</param>
        /// <param name="pessoa">A instância de Pessoa contendo os dados atualizados.</param>
        /// <exception cref="ValidationException">Lançada quando os dados da Pessoa são inválidos ou a Pessoa não é encontrada.</exception>
        public void Update(Guid Id, Pessoa pessoa)
        {
            if (!pessoa.IsValid()) throw new ValidationException(pessoa.ValidationErrors);

            if (_pessoaRepository.GetById(Id) == null) throw new ValidationException(new List<string>()
            {
                $"Pessoa com id {Id} não encontrada."
            });

            _pessoaRepository.Update(Id, pessoa);
        }

        /// <summary>
        /// Deleta uma pessoa com base no identificador fornecido. 
        /// </summary>
        /// <param name="Id">O identificador exclusivo da Pessoa a ser deletada.</param>
        /// <exception cref="ValidationException">Lançada quando a Pessoa não é encontrada.</exception>
        public void Delete(Guid Id)
        {
            if (_pessoaRepository.GetById(Id) == null) throw new ValidationException(new List<string>()
            {
                $"Pessoa com id {Id} não encontrada."
            });

            _pessoaRepository.Delete(Id);
        }

        public void Dispose()
        {
            _pessoaRepository.Dispose();
        }
    }
}
