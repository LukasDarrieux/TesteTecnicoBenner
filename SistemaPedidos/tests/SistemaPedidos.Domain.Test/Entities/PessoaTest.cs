using SistemaPedidos.Domain.Entities;
using FluentAssertions;

namespace SistemaPedidos.Domain.Test;

[TestClass]
public class PessoaTest
{
    private Endereco CriarEnderecoValido()
    {
        return new Endereco()
        {
            Rua = "Rua A",
            Numero = "123",
            Bairro = "Centro",
            Cidade = "Cidade",
            Estado = "RJ",
            CEP = "12345678"
        };
    }

    [TestMethod]
    public void Construtor_DeveRemoverMascaraDoCPF()
    {
        var pessoa = new Pessoa("Pessoa", "123.456.789-09", CriarEnderecoValido());

        pessoa.CPF.Should().Be("12345678909");
    }

    [TestMethod]
    public void AtualizarCPF_DeveRemoverMascara()
    {
        var pessoa = new Pessoa("Pessoa", "12345678909", CriarEnderecoValido());

        pessoa.AtualizarCPF("987.654.321-00");

        pessoa.CPF.Should().Be("98765432100");
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoNomeVazio()
    {
        var pessoa = new Pessoa("", "12345678909", CriarEnderecoValido());

        var resultado = pessoa.IsValid();

        resultado.Should().BeFalse();
        pessoa.ValidationErrors.Should().Contain("O nome da pessoa é obrigatório.");
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoCPFVazio()
    {
        var pessoa = new Pessoa("Pessoa", "", CriarEnderecoValido());

        var resultado = pessoa.IsValid();

        resultado.Should().BeFalse();
        pessoa.ValidationErrors.Should().Contain("O CPF da pessoa é obrigatório.");
    }

    [TestMethod]
    public void IsValid_DeveSerInvalido_QuandoCPFInvalido()
    {
        var pessoa = new Pessoa("Pessoa", "12345678900", CriarEnderecoValido());

        var resultado = pessoa.IsValid();

        resultado.Should().BeFalse();
        pessoa.ValidationErrors.Should().Contain("O CPF da pessoa é inválido.");
    }

    [TestMethod]
    public void IsValid_DeveSerValido_QuandoDadosCorretos()
    {
        // CPF válido real (gerado corretamente)
        var pessoa = new Pessoa("Pessoa", "52998224725", CriarEnderecoValido());

        var resultado = pessoa.IsValid();

        resultado.Should().BeTrue();
        pessoa.ValidationErrors.Should().BeEmpty();
    }
}
