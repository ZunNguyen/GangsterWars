using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Controller
{
    public class MoveController : MonoBehaviour
    {
        //[SerializeField] private Transform _shield;
        //[SerializeField] private Rigidbody2D _rb;
        //[SerializeField] private AnimationController _animationCtrl;

        //private void FixedUpdate()
        //{
        //    var distance = Vector2.Distance(_shield.position, transform.position);
        //    if (distance < 5f)
        //    {
        //        _rb.velocity = Vector2.zero;
        //        _animationCtrl.CurrentState.Value = AnimationState.Idle;
        //        return;
        //    }
        //    Vector2 direction = Vector2.left;
        //    _rb.velocity = direction * 1f;
        //    _animationCtrl.CurrentState.Value = AnimationState.Walk;
        //}
    }
}