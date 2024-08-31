using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Character.Leader
{
    public class Action : MonoBehaviour
    {
        [SerializeField] private Animation _animation;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Nút chuột trái
            {
                _animation.CurrentState.Value = AnimationStateLeader.Shoot_7_sprite;

                Debug.Log("a");
            }
        }
    }
}