using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour {

    [Range(0, 1)]
    public float updateFrequency;
    public Text waveCount;
    public Text monsterReserve;
    public Text resource;


    private void Start()
    {
        StartCoroutine(updateUI());
    }

    IEnumerator updateUI()
    {
        while (true)
        {
            waveCount.text = Spawn.instance.WaveCount.ToString();
            monsterReserve.text = Spawn.instance.monsterQueueCount.ToString();
            resource.text = ResourceManager.instance.Resources.ToString();      
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    public void setGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
