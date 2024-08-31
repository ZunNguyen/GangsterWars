using UnityEngine;

namespace Game.Character.Controller
{
    public abstract class AnimationController : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
    }
}