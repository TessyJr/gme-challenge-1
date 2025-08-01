using UnityEngine;

public abstract class Skill : ScriptableObject 
{
    [SerializeField] private Sprite _icon;

    public abstract void UseSkill(GameObject user);

    public Sprite GetIcon() => _icon;
}
