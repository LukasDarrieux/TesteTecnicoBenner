using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Functions;
using SistemaPedidos.Infraestrutura.Repository;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SistemaPedidos.Views.Pages
{
    /// <summary>
    /// Interação lógica para PedidoList.xam
    /// </summary>
    public partial class PedidoList : Page
    {
        public PedidoList()
        {
            InitializeComponent();
            Loaded += (s, e) => btnPesquisar_Click(null, null);
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new PedidoCrud(TipoOperacao.Cadastrar));
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Guid idPessoa = cbPessoas.SelectedItem != null ? (Guid)cbPessoas.SelectedValue : Guid.Empty;   
                StatusPedido status = cbStatus.SelectedItem != null ? (StatusPedido)cbStatus.SelectedValue : (StatusPedido)(-1);
                FormaPagamento pagamento = cbPagamento.SelectedItem != null ? (FormaPagamento)cbPagamento.SelectedValue : (FormaPagamento)(-1);
                DateTime dataInicial = txtDataInicial.SelectedDate ?? DateTime.MinValue;
                DateTime dataFinal = txtDataFinal.SelectedDate ?? DateTime.MinValue;

                if (dataInicial > dataFinal)
                {
                    MessageBox.Show("A data inicial não pode ser maior que a data final.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (PedidoHandler handler = new PedidoHandler(new PedidoRepository()))
                {
                    var pessoas = handler.Search(idPessoa, status, pagamento, dataInicial, dataFinal);
                    dataGridPedidos.ItemsSource = pessoas;
                    lblQuantidade.Text = $"Quantidade de Pessoas: {pessoas.Count}";
                }
            }
            catch(Exception erro)
            {
                MessageBox.Show("Ocorreu um erro ao pesquisar: " + erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPedidos.SelectedItem is Pedido pedidoSelecionado)
            {
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainFrame.Navigate(new PedidoCrud(TipoOperacao.Visualizar, pedidoSelecionado));
            }
            else
            {
                MessageBox.Show("Selecione uma pessoa.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPedidos.SelectedItem is Pedido pedidoSelecionado)
            {
                frmPedidoStatus statusWindow = new frmPedidoStatus(pedidoSelecionado);
                var result = statusWindow.ShowDialog();
                if (result.Value)
                {
                    btnPesquisar_Click(null, null); // Recarrega a lista após atualizar o status
                }
            }
            else
            {
                MessageBox.Show("Selecione uma pessoa.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCombo.CarregaComboPessoas(cbPessoas, true);
            LoadCombo.CarregaComboStatus(cbStatus, true);
            LoadCombo.CarregaComboPagamento(cbPagamento, true);
        }
    }
}
