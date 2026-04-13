using SistemaPedidos.Aplicacao.CustomException;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace SistemaPedidos.Aplicacao.Handlers
{
    public class ProdutoHandler : IDisposable
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        /// <summary>
        /// Pesquisa os produtos filtrando pelo nome, código e valor. 
        /// O valor é filtrado por um intervalo, 
        /// ou seja, o valor deve estar entre o valor inicial e o valor final.
        /// </summary>
        /// <param name="nome">Nome do produto</param>
        /// <param name="codigo">Código do produto</param>
        /// <param name="valorInicial">Valor inicial do intervalo</param>
        /// <param name="valorFinal">Valor final do intervalo</param>
        /// <returns>Lista de produtos que atendem aos filtros</returns>
        public List<Produto> Search(string nome, string codigo, decimal valorInicial, decimal valorFinal)
        {
            return _produtoRepository.Search(nome, codigo, valorInicial, valorFinal);
        }

        /// <summary>
        /// Pesquisa todos os produtos 
        /// </summary>
        /// <returns>Lista de produtos</returns>
        public List<Produto> Search()
        {
            return _produtoRepository.Search(string.Empty, string.Empty, 0, 0);
        }

        /// <summary>
        /// Obtém o produto correspondente ao identificador especificado.
        /// </summary>
        /// <param name="id">O identificador exclusivo do produto a ser recuperado.</param>
        /// <returns>O produto correspondente ao identificador fornecido.</returns>
        /// <exception cref="ValidationException">Lançada quando nenhum produto com o identificador especificado é encontrado.</exception>
        public Produto GetById(Guid id)
        {
            var produto = _produtoRepository.GetById(id);
            
            if (produto == null) throw new ValidationException(new List<string>()
            {
                $"Produto com id {id} não encontrado."
            });
            
            return produto;
        }

        /// <summary>
        /// Cadastra uma nova entidade de produto. Antes de criar o produto, 
        /// é realizada a validação dos dados.
        /// </summary>
        /// <param name="produto">A instância de Produto a ser criada.</param>
        /// <exception cref="ValidationException">Lançada quando os dados do Produto são inválidos.</exception>
        public void Create(Produto produto)
        {
            if (!produto.IsValid()) throw new ValidationException(produto.ValidationErrors);

            _produtoRepository.Create(produto);
        }

        /// <summary>
        /// Atualiza um produto existente com base no identificador fornecido. 
        /// Antes de atualizar o produto, é realizada a validação dos dados.
        /// </summary>
        /// <param name="id">O identificador exclusivo do produto a ser atualizado.</param>
        /// <param name="produto">A instância de Produto contendo os dados atualizados.</param>
        /// <exception cref="ValidationException">Lançada quando os dados do Produto são inválidos ou o Produto não é encontrado.</exception>
        public void Update(Guid id, Produto produto)
        {
            if (!produto.IsValid()) throw new ValidationException(produto.ValidationErrors);

            if (_produtoRepository.GetById(id) == null) throw new ValidationException(new List<string>()
            {
                $"Produto com id {id} não encontrado."
            });

            _produtoRepository.Update(id, produto);
        }

        /// <summary>
        /// Deleta um produto existente com base no identificador fornecido.
        /// </summary>
        /// <param name="id">O identificador exclusivo do produto a ser deletado.</param>
        /// <exception cref="ValidationException">Lançada quando o Produto não é encontrado.</exception>
        public void Delete(Guid id)
        {
            if (_produtoRepository.GetById(id) == null) throw new ValidationException(new List<string>()
            {
                $"Produto com id {id} não encontrado."
            });

            _produtoRepository.Delete(id);
        }

        public void Dispose()
        {
            _produtoRepository.Dispose();
        }
    }
}
