using TMPro;
using UnityEngine;


namespace Game.CanvasInGamePlay.Reload
{
    public abstract class ReloadTimeHandler : MonoBehaviour
    {
        [SerializeField] protected Transform _circle;
        [SerializeField] protected TMP_Text _time;

        protected virtual void Awake()
        {
            
        }
    }
}