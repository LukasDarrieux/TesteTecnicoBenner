using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaPedidos.Infraestrutura.Repository
{
    public class PedidoRepository  : BaseRepository<Pedido>, IPedidoRepository
    {
        private const string FILE_NAME = "pedidos.json";
        public PedidoRepository() : base(FILE_NAME)
        {
        }
        public void Create(Pedido pedido)
        {
            var list = Load();
            list.Add(pedido);
            Save(list);
        }
        public void Update(Guid id, Pedido pedido)
        {
            var list = Load();
            var index = list.FindIndex(x => x.Id == id);
            if (index >= 0)
            {
                list[index] = pedido;
                Save(list);
            }
        }
        public void Delete(Guid id)
        {
            var list = Load();
            list.RemoveAll(x => x.Id == id);
            Save(list);
        }

        public Pedido GetById(Guid id)
        {
            return Load().Find(x => x.Id == id);
        }

        public List<Pedido> Search(Guid idPessoa, StatusPedido status, FormaPagamento pagamento, DateTime dataInicial, DateTime dataFinal)
        {
            var query = Load().AsEnumerable();

            if (idPessoa != Guid.Empty)
                query = query.Where(p => p.Pessoa.Id == idPessoa);

            if (status != StatusPedido.Selecione)
                query = query.Where(p => p.Status == status);

            if (pagamento != FormaPagamento.Selecione)
                query = query.Where(p => p.Pagamento == pagamento);

            if (dataInicial != DateTime.MinValue)
                query = query.Where(p => p.DataVenda >= dataInicial);

            if (dataFinal != DateTime.MinValue)
                query = query.Where(p => p.DataVenda <= dataFinal);

            return query.ToList();
        }

        
        
    }
}
