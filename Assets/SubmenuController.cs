using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmenuController : MonoBehaviour
{
    [Header("Panels")]
    public List<GameObject> panels;

    [Header("Settings")]
    [SerializeField] bool isOpen = false;
    [SerializeField] int currIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (isOpen) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

    public void OpenSubmenu(int index) {
        isOpen = true;
        gameObject.SetActive(true);

        // Open the target submenu and close all others
        for (int i = 0; i < panels.Count; i++) {
            if (i == index) {
                panels[i].gameObject.SetActive(true);
            }
            else {
                panels[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseSubMenu() {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
