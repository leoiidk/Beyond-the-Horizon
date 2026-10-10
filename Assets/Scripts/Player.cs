using UnityEngine;

public class Player : MonoBehaviour
{
    float velocity = 5f;
    Rigidbody rb;
    [SerializeField] Camera maincamera;
    [SerializeField] Transform bossRef;

    #region Singleton

    //para fazer um singleton
    // singleton só funciona se 1 objeto unico dentro do codigo que pode ser indetificado sem precisar ser referenciado
    // similar a "deus" no codigo - ele fala e acontece - sem precisar de uma referncia
    // todos tem acesso a ele e todo mundo pode alterar e consultar ele
    // ter cuidado, só usar singleton quando for ter certeza que tiver apenas 1, não ficar criando atoa

    //criar uma var public estatica com nome da propria classe, precisa ser publica e static com o nome instancia
    public static Player instance;

    //var static uma variavel "idependente dos objetos", o valor dela se mantem o mesmo em todos os objetos da mesma classe
    //ex. bonus de XP é static nunca altera, sendo um valor fixo, enquanto o XP em si altera de inimigo pra inimigo.

    //muito util para limitar valores fixos (eg. preço em uma loja do jogo)

    private void Awake()
    {
        // pergunto se ela nao é nula e se não sou eu
        if (instance != null && instance != this)
        {
            //se existir e não for eu, mando destruir
            Destroy(instance);
        }
        else
        {
            //caso eu ainda não exista, faço existir
            instance = this;
        }
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //armazena os valores do componente na variavel rb
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimentar();
        CameraSeguir();
    }

    void Movimentar()
    {
        //Floats pegam o valor do axis
        float velX, velZ;

        //Vel X pega os valores do input horizontal do player
        velX = Input.GetAxis("Horizontal");

        //Vel Z pega os valores do input vertical do player
        velZ = Input.GetAxis("Vertical");

        //pega a velocidade rb, e aplica o valor do input + eixo (axis) na velocidade do rb, fazendo o jogador se movimentar seguindo o input + o eixo equivalente.
        rb.linearVelocity = new Vector3(velX, 0, velZ) * velocity;
        
        Vector3 trPosX, trPosZ;
        trPosX = maincamera.transform.forward + rb.linearVelocity;
        trPosZ = maincamera.transform.right + rb.linearVelocity;
    }

    void CameraSeguir()
    {
        maincamera.transform.LookAt(bossRef);

    }
}
