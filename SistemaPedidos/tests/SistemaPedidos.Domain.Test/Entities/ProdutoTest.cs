using FluentAssertions;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Domain.Test;

[TestClass]
public class ProdutoTest
{
    [TestMethod]
    public void Construtor_ComGeracaoId()
    {
        var produto = new Produto("Produto 1", "1234567890", Convert.ToDecimal(10.99));

        produto.Id.Should().NotBe(Guid.Empty);
        produto.Nome.Should().Be("Produto 1");
        produto.Codigo.Should().Be("1234567890");
        produto.Valor.Should().Be(Convert.ToDecimal(10.99));
    }

    [TestMethod]
    public void IsValid_DeveSerValido_QuantoDadosCorretos()
    {
        var produto = new Produto("Produto 1", "1234567890", Convert.ToDecimal(10.99));

        produto.IsValid().Should().BeTrue();
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoNomeVazio()
    {
        var produto = new Produto(string.Empty, "1234567890", Convert.ToDecimal(10.99));

        produto.IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoCodigoVazio()
    {
        var produto = new Produto("Produto 1", string.Empty, Convert.ToDecimal(10.99));

        produto.IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoValorZero()
    {
        var produto = new Produto("Produto 1", "1234567890", 0);

        produto.IsValid().Should().BeFalse();
    }

}
