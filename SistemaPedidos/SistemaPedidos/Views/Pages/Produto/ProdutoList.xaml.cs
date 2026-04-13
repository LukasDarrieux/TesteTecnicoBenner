using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Functions;
using SistemaPedidos.Infraestrutura.Repository;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SistemaPedidos.Views.Pages
{
    /// <summary>
    /// Interação lógica para ProdutoList.xam
    /// </summary>
    public partial class ProdutoList : Page
    {
        public ProdutoList()
        {
            InitializeComponent();
            Loaded += (s, e) => btnPesquisar_Click(null, null);
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nome = string.IsNullOrEmpty(txtNome.Text.Trim()) ? string.Empty : txtNome.Text.Trim();
                string codigo = string.IsNullOrEmpty(txtCodigo.Text.Trim()) ? string.Empty : txtCodigo.Text.Trim();
                decimal valorInicial = string.IsNullOrEmpty(txtValorInicial.Text.Trim()) ? 0 : decimal.Parse(txtValorInicial.Text.Trim());
                decimal valorFinal = string.IsNullOrEmpty(txtValorFinal.Text.Trim()) ? 0 : decimal.Parse(txtValorFinal.Text.Trim());

                if (valorInicial > valorFinal)
                {
                    MessageBox.Show("O valor inicial não pode ser maior que o valor final.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (ProdutoHandler handler = new ProdutoHandler(new ProdutoRepository()))
                {
                    var produtos = handler.Search(nome, codigo, valorInicial, valorFinal);
                    dataGridProdutos.ItemsSource = produtos;
                    lblQuantidade.Text = $"Quantidade de Produtos: {produtos.Count}";
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu um erro ao pesquisar: " + erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new ProdutoCrud(TipoOperacao.Cadastrar));
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            AbrirTelaCrud(TipoOperacao.Editar);
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProdutos.SelectedItem is Produto produtoSelecionada)
            {
                var result = MessageBox.Show($"Tem certeza que deseja excluir o produto '{produtoSelecionada.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (ProdutoHandler handler = new ProdutoHandler(new ProdutoRepository()))
                        {
                            handler.Delete(produtoSelecionada.Id);
                            MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            btnPesquisar_Click(null, null); // Recarregar a lista após exclusão
                        }
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Ocorreu um erro ao excluir: " + erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um produto para excluir.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            AbrirTelaCrud(TipoOperacao.Visualizar);
        }

        private void AbrirTelaCrud(TipoOperacao tipoOperacao)
        {
            if (dataGridProdutos.SelectedItem is Produto produtoSelecionada)
            {
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainFrame.Navigate(new ProdutoCrud(tipoOperacao, produtoSelecionada.Id));
            }
            else
            {
                MessageBox.Show("Selecione um produto para editar.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtValor_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Utils.DecimalPreviewTextInput(this, e);
        }
    }
}
