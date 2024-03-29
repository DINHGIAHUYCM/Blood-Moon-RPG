using System;
using UnityEngine;

public class MonthlyLoginSystem : MonoBehaviour
{
    [Serializable]
    public class MonthlyLoginObject
    {
        public GameObject gameObject;
        public GameObject hideObject;
        public int day;
        public string PlayerPrefsName;
    }

    public MonthlyLoginObject[] monthlyLoginObjects;

    private void Start()
    {
        DateTime currentDateTime = DateTime.Now.Date;

        foreach (MonthlyLoginObject monthlyLoginObject in monthlyLoginObjects)
        {
            if (currentDateTime.Day != monthlyLoginObject.day)
            {
                // Kích hoạt GameObject tương ứng
                monthlyLoginObject.gameObject.SetActive(false);
                monthlyLoginObject.hideObject.SetActive(true);
            }

            else if( PlayerPrefs.HasKey(monthlyLoginObject.PlayerPrefsName)){
                monthlyLoginObject.gameObject.SetActive(false);
                monthlyLoginObject.hideObject.SetActive(false);
            }

            else if (currentDateTime.Day == monthlyLoginObject.day){
                monthlyLoginObject.gameObject.SetActive(true);
                monthlyLoginObject.hideObject.SetActive(false);
            }

            else {
                monthlyLoginObject.gameObject.SetActive(true);
                monthlyLoginObject.hideObject.SetActive(true);
            }
        }
    }
}