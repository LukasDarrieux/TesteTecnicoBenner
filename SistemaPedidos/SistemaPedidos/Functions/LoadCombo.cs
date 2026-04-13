using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Infraestrutura.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SistemaPedidos.Functions
{
    public static class LoadCombo
    {
        public static void CarregaComboPessoas(ComboBox cbPessoas, bool incluirVazio = false)
        {
            using (PessoaHandler pessoahandler = new PessoaHandler(new PessoaRepository()))
            {
                List<Pessoa> pessoas = new List<Pessoa>();

                if (incluirVazio) pessoas.Add(new Pessoa(Guid.Empty, string.Empty, string.Empty, null));
                pessoas.AddRange(pessoahandler.Search(string.Empty, string.Empty));
                
                if (pessoas.Count > 0)
                {
                    cbPessoas.ItemsSource = pessoas;
                    cbPessoas.DisplayMemberPath = "Nome";
                    cbPessoas.SelectedValuePath = "Id";
                }
            }
        }
        public static void CarregaComboStatus(ComboBox cbStatus, bool incluirVazio = false)
        {
            Dictionary<StatusPedido, string> status = new Dictionary<StatusPedido, string>();
            if (incluirVazio) status.Add(StatusPedido.Selecione, string.Empty);
            status.Add(StatusPedido.Pendente, "Pendente");
            status.Add(StatusPedido.Pago, "Pago");
            status.Add(StatusPedido.Enviado, "Enviado");
            status.Add(StatusPedido.Recebido, "Recebido");

            cbStatus.ItemsSource = status;
            cbStatus.DisplayMemberPath = "Value";
            cbStatus.SelectedValuePath = "Key";
        }

        public static void CarregaComboPagamento(ComboBox cbPagamento, bool incluirVazio = false)
        {
            Dictionary<FormaPagamento, string> pagamentos = new Dictionary<FormaPagamento, string>();
            if (incluirVazio) pagamentos.Add(FormaPagamento.Selecione, string.Empty);
            pagamentos.Add(FormaPagamento.Dinheiro, "Dinheiro");
            pagamentos.Add(FormaPagamento.Cartao, "Cartão");
            pagamentos.Add(FormaPagamento.Boleto, "Boleto");
            
            cbPagamento.ItemsSource = pagamentos;
            cbPagamento.DisplayMemberPath = "Value";
            cbPagamento.SelectedValuePath = "Key";
        }
    }
}
