using Sources.DataBaseSystem;
using Sources.Services;
using Sources.Services.BootstrapLoadingService;
using Sources.UISystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private UIData _uiData;
        [SerializeField] private UIManager _uiManagerPrefab;

        [Header("DataBase")]
        [SerializeField] private DataBase _dataBase;

        private async void Start()
        {
            var bootStrapService = CreateBootstrapServiceGroup();
            await bootStrapService.Run();
        }

        private Service CreateBootstrapServiceGroup()
        {
            var serviceGroup = new SequenceServiceGroup();

            serviceGroup.Add(CreateEssentialSericeGroup());
            serviceGroup.Add(CreateBootstrapLoadingServiceGroup());
            
            return serviceGroup;
        }

        private Service CreateEssentialSericeGroup()
        {
            var serviceGroup = new SequenceServiceGroup();

            serviceGroup.Add(new InitUISystemService(_uiData, _uiManagerPrefab));
            
            return serviceGroup;
        }

        private Service CreateBootstrapLoadingServiceGroup()
        {
            var serviceGroup = new BootstrapLoadingServiceGroup();

            return serviceGroup;
        }
    }
}