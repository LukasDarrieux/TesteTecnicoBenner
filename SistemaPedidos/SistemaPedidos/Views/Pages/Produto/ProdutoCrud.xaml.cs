using SistemaPedidos.Aplicacao.CustomException;
using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Infraestrutura.Repository;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SistemaPedidos.Views.Pages
{
    /// <summary>
    /// Interação lógica para ProdutoCrud.xam
    /// </summary>
    public partial class ProdutoCrud : Page
    {
        private readonly TipoOperacao Operacao;
        private readonly Guid Id;

        public ProdutoCrud(TipoOperacao operacao)
        {
            InitializeComponent();
            Operacao = operacao;
            PreparaPage();
        }

        public ProdutoCrud(TipoOperacao operacao, Guid id)
        {
            InitializeComponent();
            Operacao = operacao;
            Id = id;
            CarregarProduto();
            PreparaPage();
        }

        private void PreparaPage()
        {
            string titulo = $"Cadastrar";

            switch (Operacao)
            {
                case TipoOperacao.Cadastrar:
                    titulo = "Cadastrar";
                    break;
                case TipoOperacao.Visualizar:
                    titulo = "Visualizar";
                    btnCancelar.Visibility = Visibility.Hidden;
                    break;
                case TipoOperacao.Editar:
                    titulo = "Editar";
                    break;
            }

            lblTitulo.Text = $"{titulo} Produto";

            if (Operacao.Equals(TipoOperacao.Visualizar)) titulo = "Fechar";
            btnSalvar.Content = titulo;
            BloquearCampos(Operacao.Equals(TipoOperacao.Visualizar) || Operacao.Equals(TipoOperacao.Excluir));
        }

        private void BloquearCampos(bool bloquear)
        {
            txtCodigo.IsReadOnly = bloquear;
            txtNome.IsReadOnly = bloquear;
            txtValor.IsReadOnly = bloquear;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            switch (Operacao)
            {
                case TipoOperacao.Cadastrar:
                    Cadastrar();
                    break;
                case TipoOperacao.Editar:
                    Editar();
                    break;
                case TipoOperacao.Visualizar:
                    btnCancelar_Click(null, null);
                    break;

            }
        }

        private void Cadastrar()
        {
            try
            {
                decimal valor = decimal.Parse(txtValor.Text);
                Produto produto = new Produto(txtNome.Text, txtCodigo.Text, valor);
                using (ProdutoHandler handler = new ProdutoHandler(new ProdutoRepository()))
                {
                    handler.Create(produto);
                }
                MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                btnCancelar_Click(null, null);
            }
            catch (ValidationException validations)
            {
                string validacoes = string.Empty;
                foreach (var validation in validations.Validations)
                {
                    validacoes += $"{validation}\n";
                }
                MessageBox.Show(validacoes, "Validações", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception excetion)
            {
                MessageBox.Show(excetion.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Editar()
        {
            try
            {
                decimal valor = decimal.Parse(txtValor.Text);
                Produto produto = new Produto(txtNome.Text, txtCodigo.Text, valor);
                using (ProdutoHandler handler = new ProdutoHandler(new ProdutoRepository()))
                {
                    handler.Update(Id, produto);
                }
                MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                btnCancelar_Click(null, null);
            }
            catch (ValidationException validations)
            {
                string validacoes = string.Empty;
                foreach (var validation in validations.Validations)
                {
                    validacoes += $"{validation}\n";
                }
                MessageBox.Show(validacoes, "Validações", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception excetion)
            {
                MessageBox.Show(excetion.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CarregarProduto()
        {
            using (ProdutoHandler handler = new ProdutoHandler(new ProdutoRepository()))
            {
                var produto = handler.GetById(Id);

                if (produto == null)
                {
                    MessageBox.Show($"Produto com Id {Id} não encontrado.", "Registro não encontrado", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                txtCodigo.Text = produto.Codigo;
                txtNome.Text = produto.Nome;
                txtValor.Text = produto.Valor.ToString("F2");
            }
        }

        private void txtValor_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Permitir apenas números e vírgula
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ",")
            {
                e.Handled = true;
            }
        }
    }
}
