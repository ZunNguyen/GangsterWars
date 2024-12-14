using Sources.FTUE.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.GameObject
{
    public interface IFTUEVieModel
    {
        public List<string> FTUEViewModelKey { get; }
    }

    public abstract class FTUEViewModelBase : MonoBehaviour, IFTUEVieModel
    {
        public List<string> FTUEViewModelKey { get; }
    }
}