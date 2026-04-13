using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Entities
{
    public class Pedido : EntidadeBase
    {
        public Pessoa Pessoa { get; set; }
        public List<ItemPedido> Itens { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataVenda { get; private set; }
        public FormaPagamento Pagamento { get; set; }
        public StatusPedido Status { get; private set; }

        [JsonConstructor]
        public Pedido(Guid id, Pessoa pessoa, List<ItemPedido> itens, DateTime dataVenda, FormaPagamento pagamento, StatusPedido status)
        {
            Id = id;
            Pessoa = pessoa;
            Itens = itens;
            CalcularValorTotal();
            DataVenda = dataVenda;
            Pagamento = pagamento;
            Status = status;
        }

        public Pedido(Pessoa pessoa, List<ItemPedido> itens, FormaPagamento pagamento)
        {
            Id = GenerateId();
            Pessoa = pessoa;
            Itens = itens;
            CalcularValorTotal();
            DataVenda = DateTime.Now;
            Pagamento = pagamento;
            Status = StatusPedido.Pendente;
        }

        public bool AtualizarStatus(StatusPedido novoStatus)
        {
            if ((short)novoStatus < (short)Status)
            {
                ValidationErrors.Add($"Não é possível atualizar status de '{Status}' para '{novoStatus}'");
                return false;
            }
            Status = novoStatus;
            return true;
        }

        public override bool IsValid()
        {
            if (Pessoa == null) ValidationErrors.Add("A pessoa é obrigatória.");
            if (Itens == null || Itens.Count == 0) ValidationErrors.Add("Pelo menos um item é obrigatório.");
            if (Pagamento < 0) ValidationErrors.Add("A forma de pagamento é obrigatória.");
            
            return ValidationErrors.Count == 0;
        }

        public void AddItem(ItemPedido item)
        {
            Itens.Add(item);
            ValorTotal += item.ValorTotal;
        }

        public void RemoveItem(ItemPedido item)
        {
            if (Itens.Remove(item))
            {
                ValorTotal -= item.ValorTotal;
            }
        }

        public void CalcularValorTotal()
        {
            ValorTotal = 0;
            foreach (var item in Itens)
            {
                ValorTotal += item.ValorTotal;
            }
        }
    }
}
