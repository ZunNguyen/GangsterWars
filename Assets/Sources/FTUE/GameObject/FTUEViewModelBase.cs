using Sirenix.OdinInspector;
using Sources.FTUE.Config;
using Sources.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.GameObject
{
    public interface IFTUEViewModel
    {
        public string FTUEViewModelKey { get; }
    }

    public abstract class FTUEViewModelBase : MonoBehaviour, IFTUEViewModel
    {
        [SerializeField, ValueDropdown(nameof(_getAllFTUEKeys))]
        private string _ftueViewModelKey;
        public string FTUEViewModelKey => _ftueViewModelKey;

        private IEnumerable _getAllFTUEKeys => IdGetter.GetAllFTUEKeyIds();
    }
}