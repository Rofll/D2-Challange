using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float Power = 0.05f, Timer = 0.5f;   // сила и продолжительность толчков
    private float shakeTimer;
    private float shakeAmount;
    public Camera mainCam;
    private Vector3 defaultCamPos;

	void Start ()
    {
        defaultCamPos = mainCam.transform.position;
    }
	

	void Update ()
    {
		if (shakeTimer >= 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;
            float oX = mainCam.transform.position.x + ShakePos.x;
            float oY = mainCam.transform.position.y + ShakePos.y;
            if (oX > 0.17) oX = 0.17f;
            else if (oX < -0.17f) oX = -0.17f;
            if (oY > 0.1f) oY = 0.1f;
            else if (oY < -0.1f) oY = -0.1f;
            mainCam.transform.position = new Vector3(oX, oY, mainCam.transform.position.z);
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // возврат камеры и проекционного расстояния на дефолтную позицию
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, 5, 2f * Time.deltaTime);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, defaultCamPos, 2f * Time.deltaTime);
        }
	}

    public void ShakeCamera()
    {
        shakeAmount = Power;
        shakeTimer = Timer;
        mainCam.orthographicSize = 4.9f;
    }
}
