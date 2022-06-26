using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField]
    private bool mine; // se o elemento é uma mina
    [SerializeField]
    private Sprite[] emptyTextures; // imagens dos blocos, menos a da mina
    [SerializeField]
    private Sprite  mineTexture; // imagem da mina
    [SerializeField]
    private Sprite  defaultTexture; // imagem da mina
    [SerializeField]
    private AudioClip audioClik; // Som do click
    [SerializeField]
    private AudioClip audioBomb; // som da bomba

    private AudioSource playSound; // tocar os sons
    // Start is called before the first frame update
    void Start()
    {
        playSound = gameObject.GetComponent<AudioSource>();
        mine = Random.value < 0.15;
        //registrar o bloco na matriz de bolcos
        int x = (int) transform.position.x; //coordenada x da tela
        int y = (int) transform.position.y; // coordenada y da tela
        GameController.elements[x,y] = this; //insere o objeto block na matriz de elementos, cujo o indece é a posição na tela.
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTexture(int adjacentCount){
        if(mine){
            
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else{
             GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }

    public void RestartDfault(){
        GetComponent<SpriteRenderer>().sprite = defaultTexture;
    }

    private void OnMouseUpAsButton(){
        if(GameController.GameState == "Play"){
            
            if(mine){
                playSound.clip = audioBomb;
                playSound.Play();
                this.LoadTexture(0); //o valor passado como parâmetro não importa, pois vai entrar no caso na mina.
                //GameController.UncoverMines(); //exibe todas as minas
                GameController.GameState = "GameOver";
            }
            else{
                playSound.clip = audioClik;
                playSound.Play();
                int x = (int) transform.position.x; //coordenada x da tela
                int y = (int) transform.position.y; // coordenada y da tela
                //lógica da proximidade da mina
                int p = GameController.adjacentMine(x,y);
                this.LoadTexture(p); // p é a quantidade de minas nas adjacencias,
                                    // e também é o indece do vetor que guarda as imagens.
                GameController.FFuncover(x,y, new bool[GameController.w, GameController.h]);
                if(GameController.IsFinished()){
                     GameController.GameState = "Win";
                //print("Você venceu!!");
                } else {
                    GameController.GameState = "PC";
                    GameController.player = "MAQ";
                }
                
            }
        }
    }


    public void playPC(int x, int y){
        if(GameController.GameState == "PC"){
            
            if(this.mine){
                this.playSound.clip = audioBomb;
                this.playSound.Play();
                this.LoadTexture(0); //o valor passado como parâmetro não importa, pois vai entrar no caso na mina.
                //GameController.UncoverMines(); //exibe todas as minas
                GameController.GameState = "GameOver";
            }
            else{
                this.playSound.clip = audioClik;
                this.playSound.Play();
                //lógica da proximidade da mina
                int p = GameController.adjacentMine(x,y);
                this.LoadTexture(p); // p é a quantidade de minas nas adjacencias,
                                    // e também é o indece do vetor que guarda as imagens.
                GameController.FFuncover(x,y, new bool[GameController.w, GameController.h]);
                if(GameController.IsFinished()){
                     GameController.GameState = "Win";
                
                }
                else{
                    //Depois que a maquina jogou, se não acertou uma mina nem ganhou
                    //o status do jogo é configurado para o usuário jogar.
                    GameController.GameState = "Play";
                    GameController.player = "USER";
                    
                }
            }
        }
    }

    public bool IsMine(){
        return this.mine; //como a mina é um valor booleano, é só retornar esse atributo do objeto Element.
    }


    //saber de um bloco ainda não foi aberto.
    public bool IsCovered(){
        bool flag = false;
        if(GetComponent<SpriteRenderer>().sprite.texture.name == "default"){
            flag = true;
        }

        return flag;
    }

}
