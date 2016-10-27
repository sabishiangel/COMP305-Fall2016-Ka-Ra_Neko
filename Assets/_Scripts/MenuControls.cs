using UnityEngine;
//reference to UI
using UnityEngine.UI;
//reference to scene manager
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuControls : MonoBehaviour
{

    //Public Instance Variables
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startbutton_Click()
    {
        SceneManager.LoadScene("Play");
    }

    public void instruction_Click()
    {
        SceneManager.LoadScene("Instructions");
    }
}
