using SistemaPedidos.Aplicacao.CustomException;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace SistemaPedidos.Aplicacao.Handlers
{
    public class PedidoHandler : IDisposable
    {
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        /// <summary>
        /// Pesquisa os pedidos de um cliente com base em filtros como status, 
        /// forma de pagamento e intervalo de datas.
        /// </summary>
        /// <param name="idPessoa">Identificador da pessoa</param>
        /// <param name="status">Status do pedido</param>
        /// <param name="pagamento">Forma de pagamento do pedido</param>
        /// <param name="dataInicial">Data inicial do intervalo</param>
        /// <param name="dataFinal">Data final do intervalo</param>
        /// <returns>Lista de pedidos que atendam aos filtros</returns>
        public List<Pedido> Search(Guid idPessoa, StatusPedido status, FormaPagamento pagamento, DateTime dataInicial, DateTime dataFinal)
        {
            return _pedidoRepository.Search(idPessoa, status, pagamento, dataInicial, dataFinal);
        }

        /// <summary>
        /// Pesquisa os pedidos 
        /// </summary>
        /// <returns>Lista de pedidos</returns>
        public List<Pedido> Search()
        {
            return _pedidoRepository.Search(Guid.Empty, StatusPedido.Selecione, FormaPagamento.Selecione, DateTime.MinValue, DateTime.MinValue);
        }

        /// <summary>
        /// Obtém o pedido correspondente ao identificador especificado.
        /// </summary>
        /// <param name="id">O identificador exclusivo do pedido a ser recuperado.</param>
        /// <returns>O pedido associado ao identificador fornecido.</returns>
        /// <exception cref="ValidationException">Lançada quando nenhum pedido com o identificador especificado é encontrado.</exception>
        public Pedido GetById(Guid id)
        {
            var pedido = _pedidoRepository.GetById(id);
            
            if (pedido == null) throw new ValidationException(new List<string>()
            {
                $"Pedido com id {id} não encontrado."
            });

            return pedido;

        }

        /// <summary>
        /// Cadastra um novo pedido no sistema. Antes de criar o pedido, 
        /// é realizada uma validação para garantir que os dados fornecidos sejam válidos.
        /// </summary>
        /// <param name="pedido">A instância de Pedido a ser criada.</param>
        /// <exception cref="ValidationException">Lançada quando os dados do Pedido são inválidos.</exception>
        public void Create(Pedido pedido)
        {
            if (!pedido.IsValid()) throw new ValidationException(pedido.ValidationErrors);
            _pedidoRepository.Create(pedido);
        }

        /// <summary>
        /// Atualiza um pedido existente no sistema.
        /// Antes de realizar a atualização, é feita uma validação para garantir que os dados fornecidos sejam válidos e que o pedido a ser atualizado exista no sistema.
        /// </summary>
        /// <param name="id">O identificador exclusivo do pedido a ser atualizado.</param>
        /// <param name="pedido">A instância de Pedido contendo os dados atualizados.</param>
        /// <exception cref="ValidationException">Lançada quando os dados do Pedido são inválidos ou o Pedido não é encontrado.</exception>
        public void Update(Guid id, Pedido pedido)
        {
            if (!pedido.IsValid()) throw new ValidationException(pedido.ValidationErrors);

            if (_pedidoRepository.GetById(id) == null) throw new ValidationException(new List<string>()
            {
                $"Pedido com id {id} não encontrado."
            });

            _pedidoRepository.Update(id, pedido);
        }

        /// <summary>
        /// Deleta um pedido existente com base no identificador fornecido.
        /// </summary>
        /// <param name="id">O identificador exclusivo do pedido a ser deletado.</param>
        /// <exception cref="ValidationException">Lançada quando o Pedido não é encontrado.</exception>
        public void Delete(Guid id)
        {
            if (_pedidoRepository.GetById(id) == null) throw new ValidationException(new List<string>()
            {
                $"Pedido com id {id} não encontrado."
            });
            _pedidoRepository.Delete(id);
        }

        public void Dispose()
        {
            _pedidoRepository.Dispose();
        }

    }
}
