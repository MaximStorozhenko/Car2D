using System.Collections;
using UnityEngine;

public class Road : MonoBehaviour
{
    private Vector3 road_start_pos;
    private Vector3 road_translation;

    public float RoadVelocity = 0.05f;

    private GameObject wall1;
    private GameObject wall2;
    private GameObject fuel;
    private GameObject heard;
    private Vector3 wall1_pos;
    private Vector3 wall2_pos;
    private Vector3 fuel_pos;
    private Vector3 heard_pos;

    private GameObject coin;
    private Vector3 coin_pos;
    private ArrayList coin_list;

    public void Start()
    {
        // запоминаем стартовую позицию (this - дороги)
        road_start_pos = this.transform.position;
        road_translation = Vector3.down * RoadVelocity;

        // находим препядствие, сохраняем ссылку
        wall1 = GameObject.Find("wall1");
        wall2 = GameObject.Find("wall2");
        wall1_pos = wall1.transform.position;
        wall2_pos = wall2.transform.position;
        wall1_pos.x = 2.4f * Random.Range(-1, 2);
        wall1_pos.y = Random.Range(-2.5f, 34.0f); // -2.5 .. 34
        wall1.transform.position = wall1_pos;

        do
        {
            wall2_pos.x = 2.4f * Random.Range(-1, 2);
            wall2_pos.y = Random.Range(-2.5f, 34.0f); // -2.5 .. 34
        }
        while (wall2_pos.x == wall1_pos.x);
        wall2.transform.position = wall2_pos;

        // находим объект fuel, сохраняем ссылку
        fuel = GameObject.Find("fuel");
        fuel_pos = fuel.transform.position;

        do
        {
            fuel_pos.x = 2.4f * Random.Range(-1, 2);
            fuel_pos.y = Random.Range(-2.5f, 34.0f);
        }
        while (
            (fuel_pos.x == wall1_pos.x && (fuel_pos.y >= (wall1_pos.y - 1.75f) && fuel_pos.y <= (wall1_pos.y + 1.75))) ||
            (fuel_pos.x == wall2_pos.x && (fuel_pos.y > (wall2_pos.y - 1.75f) && fuel_pos.y < (wall2_pos.y + 1.75)))
            );
        fuel.transform.position = fuel_pos;

        //находим объект coin
        coin = GameObject.Find("coin");
        coin_pos = coin.transform.position;

        do
        {
            coin_pos.x = 2.4f * Random.Range(-1, 2);
            coin_pos.y = Random.Range(-2.5f, 34.0f); // -2.5 .. 34
        }
        while (((coin_pos.x == fuel_pos.x) && (coin_pos.y >= (fuel_pos.y - 1.75f) && coin_pos.y <= (fuel_pos.y + 1.75f))) ||
                ((coin_pos.x == wall1_pos.x) && (coin_pos.y >= (wall1_pos.y - 1.75f) && coin_pos.y <= (wall1_pos.y + 1.75))) ||
                ((coin_pos.x == wall2_pos.x) && (coin_pos.y >= (wall2_pos.y - 1.75f) && coin_pos.y <= (wall2_pos.y + 1.75))));

        coin.transform.position = coin_pos;
        coin_list = new ArrayList();

        // находим heard
        heard = GameObject.Find("heard");
        heard_pos = heard.transform.position;
        heard.SetActive(false);

        //do
        //{
        //    heard_pos.x = 2.4f * Random.Range(-1, 2);
        //    heard_pos.y = Random.Range(-2.5f, 34.0f); // -2.5 .. 34
        //}
        //while (((heard_pos.x == fuel_pos.x) && (heard_pos.y >= (fuel_pos.y - 1.75f) && heard_pos.y <= (fuel_pos.y + 1.75f))) ||
        //        ((heard_pos.x == wall1_pos.x) && (heard_pos.y >= (wall1_pos.y - 1.75f) && heard_pos.y <= (wall1_pos.y + 1.75))) ||
        //        ((heard_pos.x == wall2_pos.x) && (heard_pos.y >= (wall2_pos.y - 1.75f) && heard_pos.y <= (wall2_pos.y + 1.75))) ||
        //        ((heard_pos.x == coin_pos.x) && (heard_pos.y >= (coin_pos.y - 1.75f) && heard_pos.y <= (coin_pos.y + 1.75))));
    }

    public void Update()
    {
        if (Menu.is_paused) return;

        // т.к. скрипт прикреплен к "дороге"
        // this - обращение к этому объекту

        this.transform.Translate(Time.deltaTime * road_translation);

        if (this.transform.position.y <= -54.08f)
        {
            if (coin_list.Count > 0)
            {
                foreach (GameObject cc in coin_list)
                    Destroy(cc);

                coin_list.Clear();
            }

            this.transform.position = road_start_pos;

            // coin
            coin.SetActive(true);
            float last_coin_copy_y = 0f;
            do
            {
                if (coin_list.Count > 0)
                {
                    foreach (GameObject cc in coin_list)
                        Destroy(cc);

                    coin_list.Clear();
                }

                coin_pos.x = 2.4f * Random.Range(-1, 2);
                coin_pos.y = Random.Range(-2.5f, 34.0f);
                coin.transform.position = coin_pos;

                int count = Random.Range(2, 5);

                for (int i = 0; i < count; i++)
                {
                    GameObject coin_copy = Instantiate(coin);
                    Vector3 coin_copy_pos = coin_copy.transform.position;
                    coin_copy_pos.y = coin_pos.y + (1.5f * (i + 1));
                    coin_copy_pos.x = coin_pos.x;
                    coin_copy.transform.position = coin_copy_pos;
                    coin_copy.transform.SetParent(this.transform);
                    coin_copy.name = "coin_clone";

                    coin_list.Add(coin_copy);

                    last_coin_copy_y = coin_copy_pos.y;
                }
            }
            while ((last_coin_copy_y + 1.75f) <= 34.0f);

            foreach (GameObject obj in coin_list)
            {
                obj.SetActive(true);
            }

            // меняем позицию препядствия -2.4 .. 0 .. 2.4
            wall1.SetActive(true);
            wall2.SetActive(true);

            do
            {
                wall1_pos.x = 2.4f * Random.Range(-1, 2);
                wall1_pos.y = Random.Range(-2.5f, 34.0f);
            }
            while ((wall1_pos.x == coin_pos.x) && (wall1_pos.y >= (coin_pos.y - 1.75f) && wall1_pos.y <= (last_coin_copy_y + 1.75f)));
            wall1.transform.position = wall1_pos;

            do
            {
                wall2_pos.x = 2.4f * Random.Range(-1, 2);
                wall2_pos.y = Random.Range(-2.50f, 34.00f); // -2.5 .. 34
            }
            while (wall2_pos.x == wall1_pos.x ||
                  (wall2_pos.x == coin_pos.x) && (wall2_pos.y >= (coin_pos.y - 1.75f) && wall2_pos.y <= (last_coin_copy_y + 1.75f)));
            wall2.transform.position = wall2_pos;

            // меняем позицию fuel'a -2.4 0 2.4
            int r = Random.Range(0, 2);
            if (r == 1)
                fuel.SetActive(true);

            do
            {
                fuel_pos.y = Random.Range(-2.5f, 34.0f);
            }
            while ((fuel_pos.x == wall1_pos.x && (fuel_pos.y >= (wall1_pos.y - 1.75f) && fuel_pos.y <= (wall1_pos.y + 1.75))) ||
                   (fuel_pos.x == wall2_pos.x && (fuel_pos.y >= (wall2_pos.y - 1.75f) && fuel_pos.y <= (wall2_pos.y + 1.75))) ||
                   (fuel_pos.x == coin_pos.x && (fuel_pos.y >= (coin_pos.y - 1.75f) && fuel_pos.y <= (last_coin_copy_y + 1.75f))));
            fuel.transform.position = fuel_pos;

            // heard
            do
            {
                heard_pos.x = 2.4f * Random.Range(-1, 2);
                heard_pos.y = Random.Range(-2.5f, 34.0f); // -2.5 .. 34
            }
            while ((heard_pos.x == fuel_pos.x && (heard_pos.y >= (fuel_pos.y - 1.75f) && heard_pos.y <= (fuel_pos.y + 1.75f))) ||
                   (heard_pos.x == wall1_pos.x && (heard_pos.y >= (wall1_pos.y - 1.75f) && heard_pos.y <= (wall1_pos.y + 1.75))) ||
                   (heard_pos.x == wall2_pos.x && (heard_pos.y >= (wall2_pos.y - 1.75f) && heard_pos.y <= (wall2_pos.y + 1.75))) ||
                   (heard_pos.x == coin_pos.x && (heard_pos.y >= (coin_pos.y - 1.75f) && heard_pos.y <= (last_coin_copy_y + 1.75))));

            int r_heard = Random.Range(1, 11);
            if (r_heard == 3 || r_heard == 7)
                heard.SetActive(true);
        }
    }
}
