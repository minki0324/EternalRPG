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
            Quaternion targetRotation = Quaternion.Euler(area.rotation.eulerAngles.x, area.rotation.eulerAngles.y, area.rotation.eulerAngles.z + 15); // 현재 x와 y 값을 유지하고 z축만 회전하기 위해 사용합니다.
            area.rotation = Quaternion.RotateTowards(area.rotation, targetRotation, 90 * Time.deltaTime); // 1초에 90도씩 회전하도록 제어합니다.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mon.isDead == true && !mon.monsterData.isElite)
        { // 몬스터가 죽어있을 때
            reviveButton.interactable = true;
            areaSprite.color = Color.green;
            reviveButton.onClick.AddListener(() => ReviveMon());
            isNearPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mon.isDead == true && !mon.monsterData.isElite)
        { // 콜라이더를 나갔을 때
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
            mon.isDead = false; // 몬스터 부활
            mon.animator.enabled = true;
            mon.InitData();

            // 부활 시켰으면 위치 옮김
            player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                                  mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);

            // 알파값 다시 올려주기
            for (int i = 0; i < mon.sprites.Length; i++)
            {
                // 몬스터의 스프라이트 알파값을 100으로 설정
                Color spriteColor = mon.sprites[i].color;
                spriteColor.a = 1f; // 알파값을 1로 설정 (100% 투명도)
                mon.sprites[i].color = spriteColor;
            }

            // area 색깔 변경
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
