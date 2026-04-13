using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Functions;
using SistemaPedidos.Infraestrutura.Repository;
using System;
using System.Windows;
using System.Windows.Input;

namespace SistemaPedidos.Views.Pages
{
    /// <summary>
    /// Lógica interna para PedidoItem.xaml
    /// </summary>
    public partial class frmPedidoItem : Window, IDisposable
    {
        public ItemPedido Item { get; private set; }

        public frmPedidoItem()
        {
            InitializeComponent();
            CarregaComboProduto();
        }

        private void CarregaComboProduto()
        {
            using(ProdutoHandler produtoHandler = new ProdutoHandler(new ProdutoRepository()))
            {
                var produtos = produtoHandler.Search();
                if (produtos.Count > 0)
                {
                    cbProdutos.ItemsSource = produtos;
                    cbProdutos.DisplayMemberPath = "Nome";
                    cbProdutos.SelectedValuePath = "Id";
                }
            }
        }

        private void txtQuantidade_PreviewTextInput(object sender, TextCompositionEventArgs e) 
        {
            Utils.NumberPreviewTextInput(sender, e);
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            var produto = cbProdutos.SelectedItem as Produto;
            var quantidade = int.Parse(txtQuantidade.Text);

            Item = new ItemPedido(produto, quantidade, produto.Valor);

            this.DialogResult = (produto != null && quantidade > 0);
        }

        public void Dispose()
        {
            
        }

        private void cbProdutos_Selected(object sender, RoutedEventArgs e)
        {
            var produto = cbProdutos.SelectedItem as Produto;
            var quantidade = 1;
            if (!string.IsNullOrEmpty(txtQuantidade.Text)) quantidade = int.Parse(txtQuantidade.Text.Trim());
            else txtQuantidade.Text = quantidade.ToString();
            
            if (produto != null)
            {
                txtPrecoUnitario.Text = produto.Valor.ToString("F2");
                txtValorTotal.Text = (produto.Valor * quantidade).ToString("F2");
            }
        }

        private void txtQuantidade_LostFocus(object sender, RoutedEventArgs e)
        {
            var produto = cbProdutos.SelectedItem as Produto;
            if (produto != null)
            {
                txtValorTotal.Text = (produto.Valor * int.Parse(txtQuantidade.Text)).ToString("F2");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
