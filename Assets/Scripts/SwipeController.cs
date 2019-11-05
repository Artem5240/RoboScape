using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour {

    bool isDragging, isMobilePlatform;
    Vector2 tapPoint, swipeDelta;  // Вторая переменная - вычисление длины свайпа
    float minSwipeDelta = 130;  // Минимальная длина свайпа

    public enum SwipeType
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public delegate void OnSwipeInput(SwipeType type);
    public static event OnSwipeInput SwipeEvent;

    private void Awake()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
            isMobilePlatform = false;
        #else
            isMobilePlatform = true;
        #endif
    }

    private void Update()
    {
        if (!isMobilePlatform)                      // Если запущено не моб. версия и зажата кнопка мыши
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                tapPoint = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))     // Если кнопка отжата
                ResetSwipe();
        }
        else
        {
                // Если запущена мобильная версия
        }
        {
            if (Input.touchCount > 0)       // Кол-во касаний > 0
            {
                if (Input.touches[0].phase == TouchPhase.Began)  // если нажатие только произошло
                {
                    isDragging = true;
                    tapPoint = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled ||    // Если нажатие отменено или закончилось
                         Input.touches[0].phase == TouchPhase.Ended)
                    ResetSwipe();
            }
        }

        CalculateSwipe();
    }

    void CalculateSwipe()
    {
        swipeDelta = Vector2.zero;        // Обнуляем дельту

        if (isDragging)    // Если тянем свайп
        {
            if (!isMobilePlatform && Input.GetMouseButton(0))       // В зависимости от платформы вычисляем длину свайпа
                swipeDelta = (Vector2)Input.mousePosition - tapPoint;
            else if (Input.touchCount > 0)
                swipeDelta = Input.touches[0].position - tapPoint;
        }

        if (swipeDelta.magnitude > minSwipeDelta)   // Если длина свайпа больше минимальной для свайпа
        {
            if (SwipeEvent != null)    // Закреплены ли за событиями обработчики   // Вычисляем направление свайпа и сообщаем что он произошел
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))    // Если х > y то свайп горизонтальный
                    SwipeEvent(swipeDelta.x < 0 ? SwipeType.LEFT : SwipeType.RIGHT);
                else                                                            // Иначе вертикальный
                    SwipeEvent(swipeDelta.y > 0 ? SwipeType.UP : SwipeType.DOWN);
            }

            ResetSwipe();
        }
    }

    void ResetSwipe()
    {
        isDragging = false;
        tapPoint = swipeDelta = Vector2.zero;
    }

}