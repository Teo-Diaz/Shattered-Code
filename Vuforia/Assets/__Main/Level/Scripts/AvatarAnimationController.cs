using UnityEngine;
using UnityEngine.Events;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onAnimationComplete;

    void OnMouseDown()
    {   
        onAnimationComplete?.Invoke();
    }
}
