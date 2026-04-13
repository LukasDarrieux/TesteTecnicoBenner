using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Infraestrutura.Repository;
using SistemaPedidos.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SistemaPedidos.Views.Pages
{
    /// <summary>
    /// Interação lógica para Home.xam
    /// </summary>
    public partial class Home : Page
    {
        private List<Pedido> Pedidos;
        private List<Produto> Produtos;
        private List<Pessoa> Pessoas;

        public Home()
        {
            InitializeComponent();
            Loaded += (s, e) => LoadHome();
        }

        private void LoadHome()
        {
            CarregaDados();
            CarregaDashboard();
            CarregaPedidoStatus();
            CarregaPedidoPagamento();
        }

        private void CarregaDados()
        {
            CarregaPedidos();
            CarregaProdutos();
            CarregaPessoas();
        }

        private void CarregaProdutos()
        {
            using (var produtoHandler = new ProdutoHandler(new ProdutoRepository()))
            {
                Produtos = produtoHandler.Search();
            }
        }

        private void CarregaPessoas()
        {
            using (var pessoaHandler = new PessoaHandler(new PessoaRepository()))
            {
                Pessoas = pessoaHandler.Search();
            }
        }

        private void CarregaPedidos()
        {
            using (var pedidoHandler = new PedidoHandler(new PedidoRepository()))
            {
                Pedidos = pedidoHandler.Search();
            }
        }

        private void CarregaDashboard()
        {
            lblTotalPedidos.Text = Pedidos.Count.ToString();
            lblTotalPessoas.Text = Pessoas.Count.ToString();
            lblTotalProdutos.Text = Produtos.Count.ToString();

        }

        private void CarregaPedidoStatus()
        {
            lblTotalPedidoPendente.Text = Pedidos.Count(p => p.Status == StatusPedido.Pendente).ToString();
            lblTotalPedidoPago.Text = Pedidos.Count(p => p.Status == StatusPedido.Pago).ToString();
            lblTotalPedidoEnviado.Text = Pedidos.Count(p => p.Status == StatusPedido.Enviado).ToString();
            lblTotalPedidoRecebido.Text = Pedidos.Count(p => p.Status == StatusPedido.Recebido).ToString();
        }

        private void CarregaPedidoPagamento()
        {
            lblTotalPagamentoDinheiro.Text = Pedidos.Count(p => p.Pagamento == FormaPagamento.Dinheiro).ToString();
            lblTotalPagamentoCartao.Text = Pedidos.Count(p => p.Pagamento == FormaPagamento.Cartao).ToString();
            lblTotalPagamentoBoleto.Text = Pedidos.Count(p => p.Pagamento == FormaPagamento.Boleto).ToString();

        }
    }
}
