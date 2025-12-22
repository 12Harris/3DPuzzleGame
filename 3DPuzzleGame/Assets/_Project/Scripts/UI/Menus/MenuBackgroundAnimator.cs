// MenuBackgroundAnimator.cs
using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundAnimator : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private RawImage backgroundImage;
    [SerializeField] private float scrollSpeedX = 0.02f;
    [SerializeField] private float scrollSpeedY = 0.01f;
    
    [Header("Fade Settings")]
    [SerializeField] private float pulseSpeed = 0.5f;
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float maxAlpha = 0.6f;
    
    private Vector2 offset;
    
    private void Update()
    {
        AnimateBackground();
        AnimateFade();
    }
    
    private void AnimateBackground()
    {
        if (backgroundImage == null) return;
        
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;
        
        backgroundImage.uvRect = new Rect(offset, backgroundImage.uvRect.size);
    }
    
    private void AnimateFade()
    {
        if (backgroundImage == null) return;
        
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, 
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        
        Color color = backgroundImage.color;
        color.a = alpha;
        backgroundImage.color = color;
    }
}
