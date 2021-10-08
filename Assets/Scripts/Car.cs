using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//test
public class Car : MonoBehaviour
{
    public Text Life;             // Ссылка на текст Life 
    public Image Indicator;       // Ссылка на Топливо
    public Text Score;            // сылка на очки
    public GameObject MenuCanvas; // ссылка на меню

    private int score;      // очки
    private int life;       // жизнь
    private float fuel;     // уровень топлива 0..1
    private float fuel_efficiency; // расход топлива
    private float fuel_tank;

    private Vector3 car_translation_left;
    private Vector3 car_translation_right;
    private float car_shift = 3.0f;

    public float CarVelocity = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        car_translation_left = Vector3.left * CarVelocity;
        car_translation_right = -car_translation_left;

        life = int.Parse(Life.text); // Стартовый счет жизни - снимает с текста
        fuel = Indicator.fillAmount;   // Стартовый уровень топлива
        fuel_efficiency = 4f / 100;

        score = int.Parse(Score.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.is_paused) return;

        /*
         * Период вызова Update хависит от вычислительной мощности
         * конкретного компьтера. Если не делать коррекции, то при
         * сетевой игре пользователи с мощьным ПК будут в выиграшной позиции.
         */

        if (Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x > -car_shift)
            this.transform.Translate(Time.deltaTime * car_translation_left);

        if (Input.GetKey(KeyCode.RightArrow) && this.transform.position.x < car_shift)
            this.transform.Translate(Time.deltaTime * car_translation_right);

        if (Menu.MenuMode == MenuMode.Game)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                MenuCanvas.SetActive(true);
                Menu.is_paused = true;
                Menu.MenuMode = MenuMode.Pause;
            }
        }

        // Уменьшаем уровень топлива
        fuel -= Time.deltaTime * fuel_efficiency;
        Indicator.fillAmount = fuel;

        // условия GameOver'a
        if(fuel <= 0 || life <= 0)
        {
            Menu.is_paused = true;
            Menu.MenuMode = MenuMode.GameOver;
            MenuCanvas.SetActive(true);
            Menu.Scores = score;

            SceneManager.LoadScene("SampleScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.SetActive(false);

        if (other.name.Equals("wall1") || other.name.Equals("wall2"))
        {
            life--;
            Life.text = life.ToString();
        }

        if (other.name.Equals("fuel"))
        {
            fuel_tank = Random.Range(0.2f, 0.5f);
            fuel = (fuel + fuel_tank) > 1 ? 1 : fuel + fuel_tank;
        }

        if (other.name.Equals("coin") || other.name.Equals("coin_clone"))
        {
            score++;
            Score.text = score.ToString();
        }

        if (other.name.Equals("heard"))
        {
            life++;
            Life.text = life.ToString();
        }
    }
}
