using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public interface IHackMenu
{
    string Name { get; }

    Action<int> Menu { get; }

    bool IsActive { get; set; }

    Rect Rect { get; set; }
}
