using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Dash Skill")]
public class DashSkill : Skill
{
    [Header("Dash Settings")]
    [SerializeField] private float _dashSpeed = 8f;
    [SerializeField] private float _dashDuration = 0.2f;

  

  

    public override void UseSkill(GameObject user)
    {
        if (!user.TryGetComponent<PlayerController>(out var player)) return;

        player.Dash(_dashSpeed, _dashDuration);

    }
}
