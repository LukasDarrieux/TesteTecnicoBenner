using SistemaPedidos.Aplicacao.CustomException;
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
    /// Interação lógica para Pessoa.xam
    /// </summary>
    public partial class PessoaCrud : Page
    {
        private readonly TipoOperacao Operacao;
        private readonly Guid Id;

        public PessoaCrud(TipoOperacao operacao)
        {
            InitializeComponent();
            Operacao = operacao;
            PreparaPage();
        }

        public PessoaCrud(TipoOperacao operacao, Guid id)
        {
            InitializeComponent();
            Operacao = operacao;
            Id = id;
            CarregarPessoa();
            PreparaPage();
        }

        private void PreparaPage()
        {
            string titulo = $"Cadastrar";

            switch(Operacao)
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

            lblTitulo.Text = $"{titulo} Pessoa";

            if (Operacao.Equals(TipoOperacao.Visualizar)) titulo = "Fechar";
            btnSalvar.Content = titulo;
            BloquearCampos(Operacao.Equals(TipoOperacao.Visualizar) || Operacao.Equals(TipoOperacao.Excluir));
        }

        private void BloquearCampos(bool bloquear)
        {
            txtBairro.IsReadOnly = bloquear;
            txtCep.IsReadOnly = bloquear;
            txtCidade.IsReadOnly = bloquear;
            txtComplemento.IsReadOnly = bloquear;
            txtCpf.IsReadOnly = bloquear;
            txtEstado.IsReadOnly = bloquear;
            txtNome.IsReadOnly = bloquear;
            txtNumero.IsReadOnly = bloquear;
            txtRua.IsReadOnly = bloquear;
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
                Endereco endereco = new Endereco()
                {
                    Bairro = txtBairro.Text,
                    CEP = txtCep.Text,
                    Cidade = txtCidade.Text,
                    Complemento = txtComplemento.Text,
                    Estado = txtEstado.Text,
                    Numero = txtNumero.Text,
                    Rua = txtRua.Text
                };

                Pessoa pessoa = new Pessoa(txtNome.Text, txtCpf.Text, endereco);
                using (PessoaHandler handler = new PessoaHandler(new PessoaRepository()))
                {
                    handler.Create(pessoa);
                }
                MessageBox.Show("Pessoa cadastrada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information); 
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
                Endereco endereco = new Endereco()
                {
                    Bairro = txtBairro.Text,
                    CEP = txtCep.Text,
                    Cidade = txtCidade.Text,
                    Complemento = txtComplemento.Text,
                    Estado = txtEstado.Text,
                    Numero = txtNumero.Text,
                    Rua = txtRua.Text
                };

                Pessoa pessoa = new Pessoa(txtNome.Text, txtCpf.Text, endereco);
                using (PessoaHandler handler = new PessoaHandler(new PessoaRepository()))
                {
                    handler.Update(Id, pessoa);
                }
                MessageBox.Show("Pessoa atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void CarregarPessoa()
        {
            using (PessoaHandler handler = new PessoaHandler(new PessoaRepository()))
            {
                var pessoa = handler.GetById(Id);

                if (pessoa == null)
                {
                    MessageBox.Show($"Pessoa com Id {Id} não encontrada.", "Registro não encontrado", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                txtBairro.Text = pessoa.Endereco.Bairro;
                txtCep.Text = pessoa.Endereco.CEP;
                txtCidade.Text = pessoa.Endereco.Cidade;
                txtComplemento.Text = pessoa.Endereco.Complemento;
                txtEstado.Text = pessoa.Endereco.Estado;
                txtNumero.Text = pessoa.Endereco.Numero;
                txtRua.Text = pessoa.Endereco.Rua;
                txtNome.Text = pessoa.Nome;
                txtCpf.Text = pessoa.CPF;
             
            }
        }

        private void txtCpf_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Utils.CpfPreviewTextInput(sender, e);
        }

        private void txtCep_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Utils.CepPreviewTextInput(sender, e);
        }
    }
}
