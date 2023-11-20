using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GUIF
{
    public static class BoxCast
    {
        public static void ChildMenu(string nameBox, Action GUIAction)
        {
            if (GUIAction == null)
                return;
            Color color = GUI.color;
            GUI.color = GUI.color + Color.white * 0.1F + new Color(0,0,0,0.1F);
            Rect rectStart = GUILayoutUtility.GetRect(100f, 20f);
            GUIAction.Invoke();
            Rect rectEnd = GUILayoutUtility.GetRect(100,5);
            GUI.color = color;
            GUI.Box(new Rect(rectStart.left - 5f, rectStart.top, rectStart.width + 10f, rectEnd.top - rectStart.top), nameBox);
        }
    }
}
