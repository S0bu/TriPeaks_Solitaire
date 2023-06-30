using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TriPeaks;
using str;
using System;

public class CardViewer : MonoBehaviour
{
    [SerializeField] GameObject[] cardDependency = new GameObject[2];
    public bool cardStatus;
    private Sprite frontFace, backFace;
    public Controller controller;

    private void Start()
    {
        int i;
        for(i = 0; i < 28; i++)
        {
            if (CustomStringFunctions.NewSubString(gameObject.name, (gameObject.name).LastIndexOf("_") + 1, gameObject.name.Length) == controller.frontFace[i].name)
                break;
        }
        frontFace = controller.frontFace[i];
        backFace = controller.backFace;
    }

    private void FixedUpdate()
    {
        Status();
        View();
    }

    private void Status()
    {
        if (cardDependency[0].activeInHierarchy == false && cardDependency[1].activeInHierarchy == false)
            cardStatus = true;
        else
            cardStatus = false;
    }
    private void View()
    {
        if (cardStatus)
        {
            gameObject.GetComponent<Button>().interactable = true;
            gameObject.GetComponent<Image>().sprite = frontFace;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().sprite = backFace;
        }
    }
}
