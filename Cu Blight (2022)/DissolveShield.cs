using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveShield : MonoBehaviour
{
    Renderer m_renderer;

    [SerializeField] float dissolveSpeed;

    private PlayerController m_player;
    private Coroutine dissolveCorourine;

    private bool shieldOn = false;

    void Start()
    {
        m_player = transform.parent.GetComponent<PlayerController>();

        m_renderer = GetComponent<Renderer>();
        OpenCloseShild();
    }

    public void OpenCloseShild()
    {
        float target = -0.6f;
        StartCoroutine(Coroutine_DissolveShield(target));
    }

    IEnumerator Coroutine_DissolveShield(float target)
    {
        float start = m_renderer.material.GetFloat("_Dissolve");
        float lerp = 0.0f;

        while(lerp < 1.0f)
        {
            // set the start height of the VFX. so, it can close/open shield
            m_renderer.material.SetFloat("_DissolveStartHeight", m_player.transform.position.y);

            m_renderer.material.SetFloat("_Dissolve", Mathf.Lerp(start, target, lerp));
            lerp += Time.deltaTime * dissolveSpeed;
            yield return null;
        }

        // to close the shield
        if (target < 0)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Coroutine_DissolveShield(1.0f));
        }
    }
}
