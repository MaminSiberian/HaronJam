using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    [SerializeField] private int givesHP;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IHealing>() != null)
        {
            collision.GetComponent<IHealing>().GetHeal(givesHP);
            gameObject.SetActive(false);
        }
    }
}
