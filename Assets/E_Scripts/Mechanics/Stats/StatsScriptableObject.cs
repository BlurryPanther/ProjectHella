using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class StatsScriptableObject : ScriptableObject
{

    //Health
    public int hp = 5;

    //Damage
    public int dmg = 3;

    //Jump
    public int jumps = 1;

    public float minFirstJumpHeight = 4;
    public float maxFirstJumpHeight = 7;

    public float minSecJumpHeight = 2;
    public float maxSecJumpHeight = 4;

    
    //Hacer un scriptable object que solo tendr� datos
    //El manager se encargar� de llamar los datos y darlos a los agentes
}
