using SistemaPedidos.Aplicacao.Handlers;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Functions;
using SistemaPedidos.Infraestrutura.Repository;
using System.Linq;
using System.Windows;

namespace SistemaPedidos.Views.Pages
{
    /// <summary>
    /// Lógica interna para frmPedidoStatus.xaml
    /// </summary>
    public partial class frmPedidoStatus : Window
    {
        private Pedido Pedido;

        public frmPedidoStatus(Pedido pedido)
        {
            InitializeComponent();
            LoadCombo.CarregaComboStatus(cbStatus);
            Pedido = pedido;
            CarregaPedido();
        }

        private void CarregaPedido()
        {
            txtPessoa.Text = Pedido.Pessoa.Nome;
            txtData.Text = Pedido.DataVenda.ToString("dd/MM/yyyy");
            txtPagamento.Text = Pedido.Pagamento.ToString();
            txtValorTotal.Text = Pedido.ValorTotal.ToString("F2");
            cbStatus.SelectedValue = Pedido.Status;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            
            if(!Pedido.AtualizarStatus((StatusPedido)cbStatus.SelectedValue))
            {
                MessageBox.Show(Pedido.ValidationErrors.FirstOrDefault(), "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.DialogResult = false;
                return;
            }
            
            using(var handler = new PedidoHandler(new PedidoRepository()))
            {
                handler.Update(Pedido.Id, Pedido);
            }
            MessageBox.Show("Status do pedido atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
        }
    }
}
