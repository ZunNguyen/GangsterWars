using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.GameObject
{
    [Serializable]
    public class ObjectData
    {
        [SerializeField] private Transform _gameObject;
        public Transform GameObject => _gameObject;

        [SerializeField] private Transform _holder;
        public Transform Holder => _holder;
    }

    public class FTUEViewModel : FTUEViewModelBase
    {
        [SerializeField] private List<ObjectData> _objectDatas;
        public List<ObjectData> ObjectDatas => _objectDatas;

        public void BackViewModelOrigin()
        {
            foreach (var objetcData in _objectDatas)
            {
                objetcData.GameObject.SetParent(objetcData.Holder);
            }
        }
    }
}