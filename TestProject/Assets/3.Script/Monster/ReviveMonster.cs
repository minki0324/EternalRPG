using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReviveMonster : MonoBehaviour
{
    [SerializeField] private Button reviveButton;
    [SerializeField] private Player player;
    private Monster mon;

    private void Awake()
    {
        mon = transform.parent.GetComponent<Monster>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mon.isDead == true)
        { // ���Ͱ� �׾����� ��
            reviveButton.interactable = true;
            reviveButton.onClick.AddListener(() => ReviveMon());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { // �ݶ��̴��� ������ ��
            reviveButton.interactable = false;
            reviveButton.onClick.RemoveListener(() => ReviveMon());
        }
    }

    public void ReviveMon()
    {
        mon.isDead = false; // ���� ��Ȱ
        mon.animator.enabled = true;
        mon.InitData();

        // ��Ȱ �������� ��ġ �ű�
        player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                              mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);
    }
}
