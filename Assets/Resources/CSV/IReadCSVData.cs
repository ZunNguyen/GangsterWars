using DG.Tweening.Plugins.Core.PathCore;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Resources.CSV
{
    public interface IReadCSVData
    {
        void ReadFile(string path);
    }
}