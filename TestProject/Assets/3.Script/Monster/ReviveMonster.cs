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
    [SerializeField] private Transform area;
    public SpriteRenderer areaSprite;

    [SerializeField] private bool isNearPlayer = false;

    private void Awake()
    {
        mon = transform.parent.GetComponent<Monster>();
        if (!mon.monsterData.isElite)
        {
            areaSprite.color = Color.yellow;
        }
        else
        {
            areaSprite.color = Color.cyan;
        }
    }

    private void Update()
    {
        if (!mon.isDead)
        { 
            Quaternion targetRotation = Quaternion.Euler(area.rotation.eulerAngles.x, area.rotation.eulerAngles.y, area.rotation.eulerAngles.z + 15); // ���� x�� y ���� �����ϰ� z�ุ ȸ���ϱ� ���� ����մϴ�.
            area.rotation = Quaternion.RotateTowards(area.rotation, targetRotation, 90 * Time.deltaTime); // 1�ʿ� 90���� ȸ���ϵ��� �����մϴ�.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mon.isDead == true && !mon.monsterData.isElite)
        { // ���Ͱ� �׾����� ��
            reviveButton.interactable = true;
            areaSprite.color = Color.green;
            reviveButton.onClick.AddListener(() => ReviveMon());
            isNearPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mon.isDead == true && !mon.monsterData.isElite)
        { // �ݶ��̴��� ������ ��
            reviveButton.interactable = false;
            areaSprite.color = Color.gray;
            reviveButton.onClick.RemoveListener(() => ReviveMon());
            isNearPlayer = false;
        }
    }

    public void ReviveMon()
    {
        if (isNearPlayer)
        {
            mon.isDead = false; // ���� ��Ȱ
            mon.animator.enabled = true;
            mon.InitData();

            // ��Ȱ �������� ��ġ �ű�
            player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                                  mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);

            // ���İ� �ٽ� �÷��ֱ�
            for (int i = 0; i < mon.sprites.Length; i++)
            {
                // ������ ��������Ʈ ���İ��� 100���� ����
                Color spriteColor = mon.sprites[i].color;
                spriteColor.a = 1f; // ���İ��� 1�� ���� (100% ����)
                mon.sprites[i].color = spriteColor;
            }

            // area ���� ����
            if (!mon.monsterData.isElite)
            {
                areaSprite.color = Color.yellow;
            }
            else
            {
                areaSprite.color = Color.cyan;
            }

            reviveButton.interactable = false;
            GameManager.Instance.DeadMonsterList.Remove(mon.monsterData.MonsterID);
        }
    }
}
