using System;
using UnityEngine;

namespace Projeteis
{
    // Míssil que sai da origem, faz um arco no ar e cai no destino depois de tempoDeVoo segundos
    [RequireComponent(typeof(Collider))]
    public class MisselEmArco : MonoBehaviour, IProjetilBoss
    {
        [SerializeField] Transform tr;

        public event Action AoFinalizar;

        private Vector3 _origem;
        private Vector3 _destino;
        private float _tempoDeVoo;
        private float _alturaDoArco;
        private float _dano;
        private float _tempoDecorrido;
        private bool _emVoo;
        private bool _finalizado;

        void Awake()
        {
            tr = transform;
            // Trigger para não empurrar o player com a física; o dano é aplicado no OnTriggerEnter
            GetComponent<Collider>().isTrigger = true;
        }

        public void Lancar(Vector3 origem, Vector3 destino, float tempoDeVoo, float alturaDoArco, float dano)
        {
            _origem = origem;
            _destino = destino;
            _tempoDeVoo = Mathf.Max(0.01f, tempoDeVoo);
            _alturaDoArco = alturaDoArco;
            _dano = dano;
            _tempoDecorrido = 0f;
            _emVoo = true;

            tr.position = origem;
        }

        void Update()
        {
            if (!_emVoo) return;

            _tempoDecorrido += Time.deltaTime;
            float t = Mathf.Clamp01(_tempoDecorrido / _tempoDeVoo);

            Vector3 novaPosicao = CalcularPosicao(t);
            Vector3 direcao = novaPosicao - tr.position;
            if (direcao.sqrMagnitude > 0.0001f)
            {
                tr.rotation = Quaternion.LookRotation(direcao);
            }
            tr.position = novaPosicao;

            if (t >= 1f)
            {
                Finalizar();
            }
        }

        // Linha reta entre origem e destino + altura de uma parábola: 4·h·t·(1−t) vale 0 nas pontas e h no meio do voo
        Vector3 CalcularPosicao(float t)
        {
            Vector3 posicao = Vector3.Lerp(_origem, _destino, t);
            posicao.y += 4f * _alturaDoArco * t * (1f - t);
            return posicao;
        }

        private void OnTriggerEnter(Collider outro)
        {
            if (!_emVoo) return;

            IDanificavel alvo = outro.GetComponentInParent<IDanificavel>();
            if (alvo == null) return;

            // Dano em aberto: o design pede HIT KILL, o valor real depende do sistema de vida do player
            alvo.ReceberDano(_dano);
            Finalizar();
        }

        void Finalizar()
        {
            if (_finalizado) return;

            _finalizado = true;
            _emVoo = false;
            AoFinalizar?.Invoke();
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            // Garante o aviso mesmo se o míssil for destruído por fora (troca de cena, etc.)
            if (!_finalizado)
            {
                _finalizado = true;
                AoFinalizar?.Invoke();
            }
        }
    }
}
