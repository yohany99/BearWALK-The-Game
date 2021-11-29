using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialogue}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Enemy enemy;
    [SerializeField] Enemy enemy1;
    [SerializeField] Enemy enemy2;
    [SerializeField] Enemy enemy3;
    [SerializeField] Enemy enemy4;
    [SerializeField] Escort escort;

    GameState state;

    private void Start()
    {
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
            //enemy.canMove = false;
            //enemy1.canMove = false;
            //enemy2.canMove = false;
            //enemy3.canMove = false;
            //enemy4.canMove = false;
        };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (state == GameState.Dialogue)
            {
                state = GameState.FreeRoam;
                //enemy.canMove = true;
                //enemy1.canMove = true;
                //enemy2.canMove = true;
                //enemy3.canMove = true;
                //enemy4.canMove = true;
            }
        };
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController?.HandleUpdate();
            enemy?.HandleUpdate();
            enemy1?.HandleUpdate();
            enemy2?.HandleUpdate();
            enemy3?.HandleUpdate();
            enemy4?.HandleUpdate();
            escort?.HandleUpdate();
        } 
        else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
