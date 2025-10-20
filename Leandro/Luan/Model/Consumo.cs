using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Luan.Models
{
    public class Consumo
    {
        public int Id { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double M3Consumidos { get; set; }
        public string Bandeira { get; set; } = string.Empty;
        public bool PossuiEsgoto { get; set; }

        public double ConsumoFaturado { get; set; }
        public double Tarifa { get; set; }
        public double ValorAgua { get; set; }
        public double AdicionalBandeira { get; set; }
        public double TaxaEsgoto { get; set; }
        public double Total { get; set; }

        public Consumo() { }

        public void CalcularValores()
        {
        
            ValorAgua = CalcularConsumo(M3Consumidos);
            AdicionalBandeira = CalcularBandeira(Bandeira, ValorAgua);
            TaxaEsgoto = CalcularEsgoto(PossuiEsgoto, ValorAgua + AdicionalBandeira);
            Total = ValorAgua + AdicionalBandeira + TaxaEsgoto;
        }

        private double CalcularConsumo(double m3Consumidos)
        {
            if (m3Consumidos <= 10)
            {
                return m3Consumidos * 2.50;
            }
            else if (m3Consumidos <= 20)
            {
                return (m3Consumidos * 3.50);
            }
            else if (m3Consumidos <= 50)
            {
                return (m3Consumidos * 5.0);
            }
            else
            {
                return (m3Consumidos * 6.50);
            }
        }

        private double CalcularBandeira(string bandeira, double consumoFaturado)
        {
            return bandeira.ToLower() switch
            {
                "verde" => 0,
                "amarela" => consumoFaturado * 0.10,
                "vermelha" => consumoFaturado * 0.20,
                _ => 0,
            };
        }

        private double CalcularEsgoto(bool possuiEsgoto, double consumoFaturado)
        {
            return possuiEsgoto ? consumoFaturado * 0.80 : 0;
        }
    }
}
