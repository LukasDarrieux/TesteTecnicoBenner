using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SistemaPedidos.Domain.Interfaces
{
    public interface IPedidoRepository : IDisposable
    {
        void Create(Pedido pedido);
        void Update(Guid id, Pedido pedido);
        void Delete(Guid id);
        Pedido GetById(Guid id);
        List<Pedido> Search(Guid idPessoa, StatusPedido status, FormaPagamento pagamento, DateTime dataInicial, DateTime dataFinal);
    }
}
