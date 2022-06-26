
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject block;
    [SerializeField]
    private AudioClip audioWin;
    [SerializeField]
    private AudioClip audioGameOver;
    [SerializeField]
    private Text msgGameOver;
    [SerializeField]
    private Text msgSpace;
    [SerializeField]
    private Text SelecionaLevel;
    [SerializeField]
    Image menu;
  

    public static int w = 10; // largura
    public static int h = 13; // altura
    public static Element[,] elements; //guardar todos os blocos
    public static bool [,] flags; //guarda todos as minas adjacentes identificadas.
    public static string GameState; //Stop, "Play", "GameOver", "Win", "PC"
    public static string player; //saber quem está jogando na hora que ganhar ou perder. ('USER', 'MAQ')
    private AudioSource playSound;
    // Start is called before the first frame update
    void Start()
    {
        GameController.GameState = "Stop"; // o jogo inicia parado
        playSound = gameObject.GetComponent<AudioSource>();
        SelecionaLevel.gameObject.active=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.GameState == "GameOver"){
            menu.enabled = true;
            msgGameOver.gameObject.active = true; // mensagem 
            if (GameController.player == "USER"){
                msgGameOver.text = "Game Over!!";
                playSound.clip = audioGameOver;
            }
            if (GameController.player == "MAQ"){
                msgGameOver.text = "Happy!!";
                playSound.clip = audioWin;
            }
            GameController.GameState = "Stop";
            playSound.Play();
        }

        if(GameController.GameState == "Win"){
            menu.enabled = true;
            msgGameOver.gameObject.active = true; // mensagem 
            
            if (GameController.player == "USER"){
                msgGameOver.text = "Happy!!";
                playSound.clip = audioWin;
            }
            if (GameController.player == "MAQ"){
                msgGameOver.text = "Game Over!!";
                playSound.clip = audioGameOver;
            }
            msgGameOver.text = "WIN!!";
            GameController.GameState = "Stop";
            playSound.clip = audioWin;
            playSound.Play();
        }
        
        if(GameController.GameState == "Stop"){
            if(SelecionaLevel.gameObject.active!=true){
            msgSpace.gameObject.active = true; // mostra a mensage na tela
            //se o usuário apertar a barra de espaço
            }
           if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)){
               //inicia o jogo
               w=8;
               h=10;
               elements = new Element[w,h];
               flags = new bool[w,h];
               this.createCamp(w,h);
             
               SelecionaLevel.gameObject.active=false;
           } 
           if(Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)){
               //inicia o jogo
               elements = new Element[w,h];
               flags = new bool[w,h];
               this.createCamp(w,h);
                SelecionaLevel.gameObject.active=false;
           } 
           if(Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)){
               //inicia o jogo
               w=18;
               h=14;
               elements = new Element[w,h];
               flags = new bool[w,h];
               this.createCamp(w,h);
                SelecionaLevel.gameObject.active=false;
               
           } 
          
              if(Input.GetKeyDown(KeyCode.Space)){
               //inicia o jogo
                StartGame();
               
           } 
        }

        if(GameController.GameState == "PC"){
            this.goPC(); 
            
        }
        
    }

    private void goPC(){
        int[] value = this.selectPlay();
        int x = value[0];
        int y = value[1];
        elements[x,y].playPC(x, y);
    }

     //Verificar se a quantidade de visinhos não abertos é igual a quantidade de minas.
    private bool signal(int x, int y){
         if(x >= 0 && y>=0 && x < w && y < h){
             if(!elements[x,y].IsCovered()){
                int neighborsMine = adjacentMine(x,y);
                int neighborsDefault = adjacentDefault(x,y);
                //print(neighborsMine +" "+ neighborsDefault);
                return neighborsMine == neighborsDefault;
             }
            
        }

        return false;
    }

    private int[] selectNotMine(){
        int[] value = {-1,-1};
        for(int x=0;x<w;x++){
            for(int y=0;y<h;y++){
               if(!elements[x,y].IsCovered() && adjacentMine(x,y) == 0){
                   if (x-1 >= 0 && y+1 >= 0 && x-1 < w && y+1 < h){
                       if(elements[x-1,y+1].IsCovered()){
                            value[0] = x-1;
                            value[1] = y+1;
                        }
                   }
                   
                    if(x >= 0 && y+1 >= 0 && x < w && y+1 < h){
                         if(elements[x,y+1].IsCovered()){
                             value[0] = x;
                            value[1] = y+1;
                         }
                        
                    } if(x+1 >= 0 && y+1 >= 0 && x+1 < w && y+1 < h){
                        if(elements[x+1,y+1].IsCovered()){
                            value[0] = x+1;
                            value[1] = y+1;
                        }
                        
                    } if(x-1 >= 0 && y >= 0 && x-1 < w && y < h){
                        if(elements[x-1,y].IsCovered()){
                            value[0] = x-1;
                            value[1] = y;
                        }
                        
                    } if(x+1 >= 0 && y >= 0 && x+1 < w && y < h){
                        if(elements[x+1,y].IsCovered()){
                            value[0] = x+1;
                            value[1] = y;
                        }
                        
                    } if(x-1 >= 0 && y-1 >= 0 && x-1 < w && y-1 < h){
                        if(elements[x-1,y-1].IsCovered()){
                            value[0] = x-1;
                            value[1] = y-1;
                        }
                        
                    } if(x >= 0 && y-1 >= 0 && x < w && y-1 < h){
                        if(elements[x,y-1].IsCovered()){
                            value[0] = x;
                            value[1] = y-1;
                        }
                        
                    } if(x+1 >= 0 && y-1 >= 0 && x+1 < w && y-1 < h){
                        if(elements[x+1,y-1].IsCovered()){
                            value[0] = x+1;
                            value[1] = y-1;
                        }
                        
                    }

               }
            }
        }

        
        return value;
    } 

    private int[] selectPlay(){
        int x;
        int y;
        int[] play = selectNotMine();
            
        if(play[0] != -1){
            return play;
        }
        else{
            bool cont = true;
            do{
                x = Random.Range(0, w);
                y = Random.Range(0, h);
                if(!GameController.flags[x,y]){
                    if(elements[x,y].IsCovered()){
                        if(this.signal(x-1,y+1)){
                            GameController.flags[x,y] = true;
                        } else if(this.signal(x,y+1)){
                            GameController.flags[x,y] = true;
                        } else if(this.signal(x+1,y+1)){
                            GameController.flags[x,y] = true;
                        } else if(this.signal(x-1,y)){
                            GameController.flags[x,y] = true;
                        } else if(this.signal(x+1,y)){
                            GameController.flags[x,y] = true;
                        } else if(this.signal(x-1,y-1)){
                            GameController.flags[x,y] = true;
                        } else if(this.signal(x,y-1)){
                            GameController.flags[x,y] = true;
                        } else if(this.signal(x+1,y-1)){
                            GameController.flags[x,y] = true;
                        }
                        
                        else{
                            cont = false;
                        }
                    
                    }
                } 
            } while(cont);

        }
            
        
        
        int[] playRandom = {x,y};
        
        return playRandom;
        
    }
    private void StartGame(){
        //alterar as imagens dos bocos
        //alterar o GameState
        GameController.GameState = "Play";
        GameController.player = "USER";
        menu.enabled = false; // retira a logo
        msgSpace.gameObject.active = false;
        msgGameOver.gameObject.active = false;
        SelecionaLevel.gameObject.active=false;
        foreach(Element item in elements){
            item.RestartDfault(); //reseta o campo minado para apresentar todos os blocos default
        }


    }

    private void createCamp(int w,int h){
        for(int i=0;i<w;i++){
            for(int j=0;j<h;j++){
                Instantiate(block, new Vector3(i,j,0f), Quaternion.identity);
            }
        }

    }

    //exibe todas as minas quando perder o jogo
    public static void UncoverMines(){
        foreach (Element item in elements){
            if(item.IsMine()){
                item.LoadTexture(0);
            }
        }
    }

    //Descobrir se o elemento é uma mina
    public static bool MineAt(int x, int y){
        bool flag = false;
        if(x >= 0 && y>=0 && x < w && y < h){
            flag = elements[x,y].IsMine();
        }

        return flag;
    }
   
   //conta quantas minhas tem nas adjacencias. 
    public static int adjacentMine(int x, int y){
        int count = 0;
        if(MineAt(x,y+1)) count++; //top
        if(MineAt(x+1,y+1)) count++; //top - right
        if(MineAt(x+1,y)) count++; //top -
        if(MineAt(x+1,y-1)) count++;
        if(MineAt(x-1,y+1)) count++;
        if(MineAt(x,y-1)) count++;
        if(MineAt(x-1,y-1)) count++;
        if(MineAt(x-1,y)) count++;
        return count;
    }

    //conta quantos visinhos ainda não foram abertos.
    public static int adjacentDefault(int x, int y){
        int count = 0;
        if(x >= 0 && y+1 >= 0 && x < w && y+1 < h && elements[x,y+1].IsCovered()) count++; //top
        if(x+1 >= 0 && y+1 >= 0 && x+1 < w && y+1 < h && elements[x+1,y+1].IsCovered()) count++; //top - right
        if(x+1 >= 0 && y >= 0 && x+1 < w && y < h && elements[x+1,y].IsCovered()) count++; //top -
        if(x+1 >= 0 && y-1 >= 0 && x+1 < w && y-1 < h && elements[x+1,y-1].IsCovered()) count++;
        if(x-1 >= 0 && y+1 >= 0 && x-1 < w && y+1 < h && elements[x-1,y+1].IsCovered()) count++;
        if(x >= 0 && y-1 >= 0 && x < w && y-1 < h && elements[x,y-1].IsCovered()) count++;
        if(x-1 >= 0 && y-1 >= 0 && x-1 < w && y-1 < h && elements[x-1,y-1].IsCovered()) count++;
        if(x-1 >= 0 && y >= 0 && x-1 < w && y < h && elements[x-1,y].IsCovered()) count++;
        return count;
    }

    //Algoritmo FloodFill
    public static void  FFuncover(int x, int y, bool[,] visited){
        if(x >= 0 && y >= 0 && x < w && y < h){
            if(visited[x,y]){
                return; // Condição de parada da recursão
            }
            else{
                //altera a imagem do boco.
                elements[x,y].LoadTexture(adjacentMine(x,y));
                //se o bloco tiver um mina adjacente, pare.
                if(adjacentMine(x,y) > 0){
                    return;
                }
                visited[x,y] = true;

                FFuncover(x-1, y, visited);
                FFuncover(x+1, y, visited);
                FFuncover(x, y-1, visited);
                FFuncover(x, y+1, visited);
                //FFuncover(x+1, y+1, visited);
                //FFuncover(x-1, y-1, visited);

            }
        }

    }

    //saber se o jogo acabou
    public static bool IsFinished(){
        foreach(Element item in elements){
            //se o elemento ainda não foi visitado e não é uma mina
            //então o jogo não acabou.
            if(item.IsCovered() && !item.IsMine()){
                return false;
            } 
        }

        return true;
    }

}
