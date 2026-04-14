using FluentAssertions;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Test;

[TestClass]
public class PedidoTest
{

    private Pessoa GetPessoa()
    {
        return new Pessoa("Pessoa", "69544087001", null);
    }

    private Produto GetProduto()
    {
        return new Produto("Produto 1", "1234567890", Convert.ToDecimal(10.99));
    }

    private ItemPedido GetItemPedido()
    {
        var produto = GetProduto();
        return new ItemPedido(produto, 2, produto.Valor);
    }

    [TestMethod]
    public void Construtor_ComGeracaoId()
    {
        
        var listaItens = new List<ItemPedido>();
        listaItens.Add(GetItemPedido());

        var pedido = new Pedido(GetPessoa(), listaItens, FormaPagamento.Cartao);

        pedido.Id.Should().NotBe(Guid.Empty);
    }


    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoPessoaNull()
    {

        var listaItens = new List<ItemPedido>();
        listaItens.Add(GetItemPedido());

        var pedido = new Pedido(null, listaItens, FormaPagamento.Cartao);

        pedido.IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoItensCountZero()
    {
        var listaItens = new List<ItemPedido>();
        var pedido = new Pedido(GetPessoa(), listaItens, FormaPagamento.Cartao);

        pedido.IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoFormaPagamentoNaoInformado()
    {
        var listaItens = new List<ItemPedido>();
        listaItens.Add(GetItemPedido());

        var pedido = new Pedido(GetPessoa(), listaItens, FormaPagamento.Selecione);

        pedido.IsValid().Should().BeFalse();
    }

}
