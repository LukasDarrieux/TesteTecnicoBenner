namespace SistemaPedidos.Domain.Entities
{
    public class ItemPedido
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal => Quantidade * ValorUnitario;
        public ItemPedido(Produto produto, int quantidade, decimal valorUnitario)
        {
            Produto = produto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }
    }
}
