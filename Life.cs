using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int scoreGive = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Game.obj.addScore(scoreGive);
            Player.obj.addLive();
            AudioManager.obj.playCoin();

            UiManager.obj.updateScore();
            UiManager.obj.updateLives();

            FXManager.obj.showPop(transform.position);
            gameObject.SetActive(false);
        }
    }






}
