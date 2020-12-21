using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    private static Color[] colorsList = new Color[4] //список цветов игрока
    {
        Color.red,
        Color.green,
        new Color(0, 0.863f, 1),
        new Color(1, 1, 0)
    };

    public GameObject player; //игрок
    private PlayerController playerController; //скрипт игрока
    public Vector3 playerPosition; //изначальная позиция игрока

    public GameObject canvasMain; //главное меню
    public GameObject canvasSettings; //окно с настройками
    public GameObject canvasPlaySure; //окно с подтверждением
    public GameObject canvasRecords; //окно с таблицей рекордов

    public Text countText; //счётчик очков
    public Button buttonStart; //кнопка запуска
    public Button buttonSettings; //кнопка настроек
    public Button buttonExit; //кнопка выхода
    public Button buttonApply; //кнопка применения настроек
    public Button buttonYES; //кнопка подтверждения
    public Button buttonNO; //кнопка отмены
    public Button buttonAccept; //кнопка подтверждения таблицы

    public string playerName; //имя игрока
    public GameObject inputName; //поле ввода имени
    public GameObject inputColor; //поле ввода цвета

    private int count; //текущее количество очков
    public int maxCount; //максимальное количество очков

    private float currentTime; //текущее время
    private List<string> recNames; //таблица имён
    private List<float> recTimes; //таблица времён
    public Text[] recTexts; //строки таблицы

    public bool playOrExit; //за что отвечает кнопка YES

    // Start is called before the first frame update
    void Start()
    {
        //сброс таблицы рекордов
        recNames = new List<string>() { "<empty>", "<empty>", "<empty>", "<empty>", "<empty>", "<empty>", "<empty>", "<empty>", "<empty>", "<empty>" };
        recTimes = new List<float>() { 999, 999, 999, 999, 999, 999, 999, 999, 999, 999 };

        playerController = player.GetComponent<PlayerController>();
        playerPosition = player.transform.position;

        buttonStart.onClick.AddListener(delegate
        {
            playOrExit = true;

            //переключение на окно с подтверждением
            canvasMain.SetActive(false);
            canvasPlaySure.SetActive(true);
        });
        buttonSettings.onClick.AddListener(delegate
        {
            //переключение на окно с настройками
            canvasMain.SetActive(false);
            canvasSettings.SetActive(true);
        });
        buttonExit.onClick.AddListener(delegate
        {
            playOrExit = false;

            //переключение на окно с подтверждением
            canvasMain.SetActive(false);
            canvasPlaySure.SetActive(true);
        });
        buttonApply.onClick.AddListener(delegate
        {
            //установка имени игрока
            playerName = inputName.GetComponent<InputField>().text;

            //установка цвета игрока
            Dropdown colorMenu = inputColor.GetComponent<Dropdown>();
            player.GetComponent<Renderer>().material.color = colorsList[colorMenu.value];

            //возврат к предыдущему окну
            canvasMain.SetActive(true);
            canvasSettings.SetActive(false);
        });
        buttonYES.onClick.AddListener(delegate
        {
            //возврат к предыдущему окну
            canvasMain.SetActive(true);
            canvasPlaySure.SetActive(false);

            if (playOrExit)
                startGame();
            else
                Application.Quit();
        });
        buttonNO.onClick.AddListener(delegate
        {
            //возврат к предыдущему окну
            canvasMain.SetActive(true);
            canvasPlaySure.SetActive(false);
        });
        buttonAccept.onClick.AddListener(delegate
        {
            //скрытие таблицы рекордов
            canvasRecords.SetActive(false);

            //отображение кнопок меню
            buttonStart.gameObject.SetActive(true);
            buttonSettings.gameObject.SetActive(true);
            buttonExit.gameObject.SetActive(true);
        });
    }

    //начать игру
    public void startGame()
    {
        count = 0;
        SetCountText();
        resetLocation();
        currentTime = Time.time;

        //разблокировака игрока
        playerController.canMove = true;

        //сокрытие кнопок меню
        buttonStart.gameObject.SetActive(false);
        buttonSettings.gameObject.SetActive(false);
        buttonExit.gameObject.SetActive(false);
    }

    //сбросить игровую локацию
    private void resetLocation()
    {
        player.transform.position = playerPosition;
        playerController.restorePickUps();
    }

    //засчитать подобранный элемент
    public void incrementCount()
    {
        count++;
        SetCountText();

        //если игрок победил
        if (count >= maxCount)
        {
            //блокировка игрока
            playerController.canMove = false;

            //отображение таблицы рекордов
            InsertRecord(playerName, Time.time - currentTime);
            for (int i = 0; i < 10; i++)
                recTexts[i].text = recNames[i] + ": " + recTimes[i];
            canvasRecords.SetActive(true);
        }
    }

    //установить текст счётчика
    private void SetCountText()
    {
        countText.text = "Score: " + count;
    }

    //вставить запись в таблицу рекордов
    private void InsertRecord(string name, float time)
    {
        int position = 0; //позиция вставки

        //найти позицию вставки в списке
        for (int i = 0; i < 10; i++)
            if (recTimes[i] < time)
                position++;

        //вставить запись
        recNames.Insert(position, name);
        recTimes.Insert(position, time);
    }
}
