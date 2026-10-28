using System.Collections.Generic;
using Commander.Ataques;
using UnityEngine;

namespace Commander
{
    // Sorteia o próximo ataque da fase, respeitando os pesos e o PodeExecutar() de cada um
    public class SeletorDeAtaquePonderado
    {
        private readonly List<List<(IAtaqueBoss ataque, float peso)>> _ataquesPorFase = new();
        private readonly List<(IAtaqueBoss ataque, float peso)> _candidatos = new();

        public SeletorDeAtaquePonderado(BossConfig config)
        {
            // Um ataque usado em várias fases vira uma única instância da estratégia
            var instancias = new Dictionary<AtaqueConfig, IAtaqueBoss>();

            foreach (FaseBoss fase in config.Fases)
            {
                var ataquesDaFase = new List<(IAtaqueBoss, float)>();

                foreach (AtaquePonderado entrada in fase.Ataques)
                {
                    if (entrada.Ataque == null) continue;

                    if (!instancias.TryGetValue(entrada.Ataque, out IAtaqueBoss ataque))
                    {
                        ataque = entrada.Ataque.CriarAtaque();
                        instancias.Add(entrada.Ataque, ataque);
                    }
                    ataquesDaFase.Add((ataque, entrada.Peso));
                }

                _ataquesPorFase.Add(ataquesDaFase);
            }
        }

        public IAtaqueBoss Sortear(int fase, ContextoAtaque contexto)
        {
            if (fase < 0 || fase >= _ataquesPorFase.Count) return null;

            _candidatos.Clear();
            float pesoTotal = 0f;

            foreach (var entrada in _ataquesPorFase[fase])
            {
                if (entrada.peso <= 0f || !entrada.ataque.PodeExecutar(contexto)) continue;

                _candidatos.Add(entrada);
                pesoTotal += entrada.peso;
            }

            if (_candidatos.Count == 0) return null;

            float sorteio = Random.Range(0f, pesoTotal);
            foreach (var candidato in _candidatos)
            {
                sorteio -= candidato.peso;
                if (sorteio <= 0f) return candidato.ataque;
            }

            return _candidatos[_candidatos.Count - 1].ataque;
        }
    }
}
