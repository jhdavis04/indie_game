using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TypeChart", menuName = "RPG/Type Chart")]
public class TypeChart : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public ElementType type;
        public List<ElementType> weaknesses;
        public List<ElementType> resistances;
    }
    public List<Entry> chart;

    public float GetMultiplier(ElementType att, ElementType def)
    {
        var entry = chart.Find(e => e.type == def);
        if (entry.weaknesses.Contains(att)) return 2f;
        if (entry.resistances.Contains(att)) return 0.5f;
        return 1f;
    }
}