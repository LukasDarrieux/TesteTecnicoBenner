using Newtonsoft.Json;
using System;

namespace SistemaPedidos.Domain.Entities
{
    public class Produto : EntidadeBase
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public decimal Valor { get; set; }

        [JsonConstructor]
        public Produto(Guid id, string nome, string codigo, decimal valor)
        {
            Id = id;
            Nome = nome;
            Codigo = codigo;
            Valor = valor;
        }

        public Produto(string nome, string codigo, decimal valor)
        {
            Id = GenerateId();
            Nome = nome;
            Codigo = codigo;
            Valor = valor;
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(Nome)) ValidationErrors.Add("O nome do produto é obrigatório.");
            if (string.IsNullOrEmpty(Codigo)) ValidationErrors.Add("O código do produto é obrigatório.");
            if (Valor <= 0) ValidationErrors.Add("O valor do produto deve ser maior que zero.");

            return ValidationErrors.Count == 0;
        }
    }
}