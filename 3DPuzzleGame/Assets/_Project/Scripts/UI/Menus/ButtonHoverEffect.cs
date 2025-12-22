// ButtonHoverEffect.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    [SerializeField] private float hoverScale = 1.05f;
    [SerializeField] private float scaleDuration = 0.15f;
    
    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(1f, 0.9f, 0.5f);
    
    [Header("Audio")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    
    private Vector3 originalScale;
    private Image buttonImage;
    private TextMeshProUGUI buttonText;
    private Button button;
    private AudioSource audioSource;
    private Coroutine scaleCoroutine;
    
    private void Awake()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;
        
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        
        scaleCoroutine = StartCoroutine(ScaleButton(originalScale * hoverScale));
        
        if (buttonText != null)
            buttonText.color = hoverColor;
        
        PlaySound(hoverSound);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;
        
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        
        scaleCoroutine = StartCoroutine(ScaleButton(originalScale));
        
        if (buttonText != null)
            buttonText.color = normalColor;
    }
    
    private System.Collections.IEnumerator ScaleButton(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;
        
        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scaleDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        transform.localScale = targetScale;
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
    
    public void PlayClickSound()
    {
        PlaySound(clickSound);
    }
}
