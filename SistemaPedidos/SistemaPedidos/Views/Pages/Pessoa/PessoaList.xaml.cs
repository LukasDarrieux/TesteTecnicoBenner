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
    /// Interação lógica para PessoaList.xam
    /// </summary>
    public partial class PessoaList : Page
    {
        public PessoaList()
        {
            InitializeComponent();
            Loaded += (s, e) => btnPesquisar_Click(null, null);
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nome = string.IsNullOrEmpty(txtNome.Text.Trim()) ? string.Empty : txtNome.Text.Trim();
                string cpf = string.IsNullOrEmpty(txtCpf.Text.Trim()) ? string.Empty : RemoveMascaraCPF(txtCpf.Text.Trim());

                using (PessoaHandler handler = new PessoaHandler(new PessoaRepository()))
                {
                    var pessoas = handler.Search(nome, cpf);
                    dataGridPessoas.ItemsSource = pessoas;
                    lblQuantidade.Text = $"Quantidade de Pessoas: {pessoas.Count}";
                }
            }
            catch(Exception erro)
            {
                MessageBox.Show("Ocorreu um erro ao pesquisar: " + erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            AbrirTelaCrud(TipoOperacao.Editar);
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            AbrirTelaCrud(TipoOperacao.Visualizar);
        }

        private void AbrirTelaCrud(TipoOperacao tipoOperacao)
        {
            if (dataGridPessoas.SelectedItem is Pessoa pessoaSelecionada)
            {
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainFrame.Navigate(new PessoaCrud(tipoOperacao, pessoaSelecionada.Id));
            }
            else
            {
                MessageBox.Show("Selecione uma pessoa.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPessoas.SelectedItem is Pessoa pessoaSelecionada)
            {
                var result = MessageBox.Show($"Tem certeza que deseja excluir a pessoa '{pessoaSelecionada.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (PessoaHandler handler = new PessoaHandler(new PessoaRepository()))
                        {
                            handler.Delete(pessoaSelecionada.Id);
                            MessageBox.Show("Pessoa excluída com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("Selecione uma pessoa para excluir.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string RemoveMascaraCPF(string cpf)
        {
            return cpf.Trim().Replace(".", "").Replace("-", "").Replace(",", "");
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new PessoaCrud(TipoOperacao.Cadastrar));
        }

        private void btnPedido_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPessoas.SelectedItem is Pessoa pessoaSelecionada)
            {
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainFrame.Navigate(new PedidoCrud(pessoaSelecionada));
            }
            else
            {
                MessageBox.Show("Selecione uma pessoa para criar um pedido.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtCpf_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Utils.CpfPreviewTextInput(sender, e);
        }
    }
}
