using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievimentView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private void OnEnable()
    {
        animator.Play("popup");

        StartCoroutine(TurnOff());
    }

    public void SetPopupText(string text)
    {
        textMeshPro.text = text;
    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(2f);
        animator.Play("idle");
        gameObject.SetActive(false);
    }
}
