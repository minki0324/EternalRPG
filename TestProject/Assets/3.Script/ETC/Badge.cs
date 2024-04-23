using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Badge : MonoBehaviour
{
    [SerializeField] private Image badgeImage;
    [SerializeField] private TMP_Text data1;
    [SerializeField] private TMP_Text data2;
    [SerializeField] private TMP_Text infoText;

    private void OnEnable()
    {
        badgeImage.sprite = EquipmentManager.Instance.GetBadgeSprite(GameManager.Instance.BadgeData);
        string NextGrade = GameManager.Instance.BadgeData.NextGrade == 0 ? "최고 등급" : $"{GameManager.Instance.BadgeData.NextGrade - DataManager.Instance.GetOwnCount()}";
        infoText.text = $"총 컬렉션 : {DataManager.Instance.GetOwnCount()} / {DataManager.Instance.TotalOwnCount()}\n" +
                                     $"현재 등급 : {GameManager.Instance.BadgeData.BadgeName} 뱃지\n" +
                                     $"다음 등급까지 : {NextGrade}";
        data1.text = $": {GameManager.Instance.BadgeData.BadgeATKPercent}%\n" +
                             $": {GameManager.Instance.BadgeData.BadgeAvoidResist}%\n" +
                             $": {GameManager.Instance.BadgeData.BadgeSTRPercent}%\n" +
                             $": {GameManager.Instance.BadgeData.BadgeDEXPercent}%\n" +
                             $": {GameManager.Instance.BadgeData.BadgeLUCPercent}%\n" +
                             $": {GameManager.Instance.BadgeData.BadgeVITPercent}%";
        data2.text = $": {GameManager.Instance.BadgeData.BadgeBonusEnergy}\n" +
                              $": {GameManager.Instance.BadgeData.BadgeBonusAP}\n" +
                              $": {GameManager.Instance.BadgeData.BadgeMoveSpeed}\n" +
                              $": {GameManager.Instance.BadgeData.BadgeEXPPercent}%\n" +
                              $": {GameManager.Instance.BadgeData.BadgeGoldPercent}%\n" +
                              $": {GameManager.Instance.BadgeData.BadgeItemDropRate}%\n" +
                              $": {GameManager.Instance.BadgeData.BadgeRuneDrop}%";
    }
}
