using SistemaPedidos.Views.Pages;
using System;
using System.Windows;

namespace SistemaPedidos
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Home_Click(this, null);
        }

        public void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Home());
        }

        private void Pessoa_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PessoaList());
        }

        private void Produto_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProdutoList());
        }

        private void Pedido_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PedidoList());
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
