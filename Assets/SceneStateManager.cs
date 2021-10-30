using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public Button btnReset;

    // Start is called before the first frame update
    void Start()
    {
        btnReset.onClick.AddListener(ResetScene);
    }

    private void ResetScene() {
        SceneManager.LoadScene(SceneType.GAME.ToString());
    }
}