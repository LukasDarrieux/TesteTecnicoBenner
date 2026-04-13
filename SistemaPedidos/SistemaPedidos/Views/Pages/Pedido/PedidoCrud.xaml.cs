using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Functions;
using SistemaPedidos.Infraestrutura.Repository;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SistemaPedidos.Views.Pages
{
    /// <summary>
    /// Interação lógica para PedidoCrud.xam
    /// </summary>
    public partial class PedidoCrud : Page
    {
        private readonly TipoOperacao Operacao;
        private readonly Pedido Pedido;

        public PedidoCrud(TipoOperacao operacao)
        {
            InitializeComponent();
            Operacao = operacao;
            PreparaTela();
        }

        public PedidoCrud(TipoOperacao operacao, Pedido pedido)
        {
            InitializeComponent();
            Operacao = operacao;
            Pedido = pedido;
            PreparaTela();
        }

        public PedidoCrud(Pessoa pessoa)
        {
            InitializeComponent();
            Operacao = TipoOperacao.Cadastrar;
            PreparaTela();
            cbPessoa.SelectedItem = pessoa;
            cbPessoa.Text = pessoa.Nome;
        }

        private void PreparaTela()
        {
            LoadCombo.CarregaComboPessoas(cbPessoa);
            LoadCombo.CarregaComboPagamento(cbPagamento);

            switch (Operacao)
            {
                case TipoOperacao.Cadastrar:
                    lblTitulo.Text = "Cadastrar Pedido";
                    txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    break;
             
                case TipoOperacao.Visualizar:
                    lblTitulo.Text = "Visualizar Pedido";
                    CarregarPedido();
                    BloquearCampos();
                    break;
            }
            
        }

        private void CarregarPedido()
        {
            try
            {
                txtData.Text = Pedido.DataVenda.ToString("dd/MM/yyyy");
                cbPessoa.SelectedValue = Pedido.Pessoa.Id;
                
                cbPagamento.SelectedValue = Pedido.Pagamento;
                foreach (var item in Pedido.Itens)
                {
                    dataGridItens.Items.Add(item);
                }
                CalcularTotal();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar pedido: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BloquearCampos()
        {
            cbPessoa.IsEnabled = false;
            cbPagamento.IsEnabled = false;
            dataGridItens.IsReadOnly = true;
            btnSalvar.Content = "Fechar";
            btnCancelar.Visibility = Visibility.Collapsed;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (Operacao.Equals(TipoOperacao.Cadastrar))
                Salvar();
            else 
                btnCancelar_Click(null, null);
        }

        private void Salvar()
        {
            try
            {
                var pessoa = cbPessoa.SelectedItem as Pessoa;
                var itens = dataGridItens.Items.OfType<ItemPedido>().ToList();
                var pagamento = cbPagamento.SelectedItem == null ? 0 : (FormaPagamento)cbPagamento.SelectedItem;
                Pedido pedido = new Pedido(pessoa, itens, pagamento);
                if (!pedido.IsValid())
                {
                    MessageBox.Show(string.Join(Environment.NewLine, pedido.ValidationErrors), "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (PedidoHandler pedidoHandler = new PedidoHandler(new PedidoRepository()))
                {
                    pedidoHandler.Create(pedido);
                }
                MessageBox.Show("Pedido cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                btnCancelar_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar pedido: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if (Operacao.Equals(TipoOperacao.Visualizar)) return;

            var frmPedidoItem = new frmPedidoItem();
            var result = frmPedidoItem.ShowDialog();

            if (result.Value && frmPedidoItem.Item != null)
            {
                var pedidoItem = frmPedidoItem.Item;
                
                if (pedidoItem != null)
                {
                    if (ItemJaAdicionado(pedidoItem))
                    {
                        MessageBox.Show("O produto já adicionado na lista!", "Produto repetido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    dataGridItens.Items.Add(pedidoItem);
                    CalcularTotal();
                }
            }
            
        }

        private bool ItemJaAdicionado(ItemPedido novoItem)
        {
            return dataGridItens.Items
                                .OfType<ItemPedido>()
                                .Any(item => item.Produto.Id == novoItem.Produto.Id);
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            if (Operacao.Equals(TipoOperacao.Visualizar)) return;
            
            if (dataGridItens.SelectedItem != null)
            {
                dataGridItens.Items.Remove(dataGridItens.SelectedItem);
                CalcularTotal();
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (var item in dataGridItens.Items)
            {
                if (item is ItemPedido pedidoItem)
                {
                    total += pedidoItem.ValorTotal;
                }
            }
            lblQuantidade.Content = $"Quantidade: {dataGridItens.Items.Count}";
            lblValorTotal.Content = $"Sub Total: {total.ToString("C")}";
        }
    }
}
