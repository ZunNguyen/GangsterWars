using Sources.DataBaseSystem;
using Sources.Services;
using Sources.UISystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private DataBase _dataBase;
        [SerializeField] private UIData _uiData;

        private async void Start()
        {
            var bootStrapService = CreateBootstrapServiceGroup();
            await bootStrapService.Run();
        }

        private Service CreateBootstrapServiceGroup()
        {
            var serviceGroup = new SequenceServiceGroup();
            return serviceGroup;
        }
    }
}