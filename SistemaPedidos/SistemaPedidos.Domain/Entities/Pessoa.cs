using Newtonsoft.Json;
using System;

namespace SistemaPedidos.Domain.Entities
{
    public class Pessoa : EntidadeBase
    {
        public string Nome { get; set; }
        public string CPF { get; private set; }
        public Endereco Endereco { get; set; }

        [JsonConstructor]
        public Pessoa(Guid id, string nome, string cpf, Endereco endereco)
        {
            Id = id;
            Nome = nome;
            CPF = RemoveMascaraCPF(cpf);
            Endereco = endereco;
        }

        public Pessoa(string nome, string cpf, Endereco endereco)
        {
            Id = GenerateId();
            Nome = nome;
            CPF = RemoveMascaraCPF(cpf);
            Endereco = endereco;
        }

        public void AtualizarCPF(string novoCPF)
        {
            CPF = RemoveMascaraCPF(novoCPF);
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(Nome)) ValidationErrors.Add("O nome da pessoa é obrigatório.");
            if (string.IsNullOrEmpty(CPF)) ValidationErrors.Add("O CPF da pessoa é obrigatório.");
            else if (!IsValidCPF(CPF)) ValidationErrors.Add("O CPF da pessoa é inválido.");
            
            return ValidationErrors.Count == 0;
        }

        private bool IsValidCPF(string cpf)
        {
            cpf = RemoveMascaraCPF(cpf);

            // Implementação simplificada de validação de CPF
            if (cpf.Length != 11) return false;
            // Verificar se todos os dígitos são iguais (ex: 111.111.111-11)
            if (new string(cpf[0], cpf.Length) == cpf) return false;
            // Cálculo dos dígitos verificadores
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        private string RemoveMascaraCPF(string cpf)
        {
            return cpf.Trim().Replace(".", "").Replace("-", "").Replace(",", "");
        }

    }
}
