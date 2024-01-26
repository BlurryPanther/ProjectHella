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

    public int minFirstJumpHeight = 4;
    public int maxFirstJumpHeight = 7;

    public int minSecJumpHeight = 2;
    public int maxSecJumpHeight = 4;

    
    //Hacer un scriptable object que solo tendrá datos
    //El manager se encargará de llamar los datos y darlos a los agentes
}
