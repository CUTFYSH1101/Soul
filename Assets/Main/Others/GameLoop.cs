using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Main.Game;
using Main.Util;
using UnityEngine;

namespace Main.Others
{
    public class GameLoop : MonoBehaviour
    {
        private Rigidbody2D _unityRb2D;
        public float force;
        private bool _first;
        private bool _second;
        private float v0;
        private float drag;

        private void Awake()
        {
            _unityRb2D = this.GetOrAddComponent<Rigidbody2D>();
            double tt;
            Efficiency.UnityEventTest(() => tt = 16.09 * Math.Pow(5, -0.654), 1000000); //  x^y, 差
            Efficiency.UnityEventTest(() => tt = -0.214 * Math.Log(5, Math.E) + 1.6283, 1000000); //  logy(x), 差
            /*
            double tt;
            Efficiency.UnityEventTest(() => tt = 5 + 5, 1000000); // 優
            Efficiency.UnityEventTest(() => tt = 5 - 5, 1000000); // 優
            Efficiency.UnityEventTest(() => tt = 5 * 5, 1000000); // 優
            Efficiency.UnityEventTest(() => tt = 5.0 / 2.0, 1000000); // 優
            Efficiency.UnityEventTest(() => tt = 5 ^ 5, 1000000); // 優
            Efficiency.UnityEventTest(() => tt = Math.Pow(5, 5), 1000000); //  x^y, 差
            Efficiency.UnityEventTest(() => tt = Math.Log(5, 5), 1000000); //  logy(x), 差
            Efficiency.UnityEventTest(() => tt = Math.Log10(5), 1000000); //  log10(x), 好
            Efficiency.UnityEventTest(() => tt = Math.Exp(5), 1000000); //  e^x, 好
            Efficiency.UnityEventTest(() => tt = 5!, 1000000); // 優
            */
            /*
            Debug.Log(Math.Pow(5, 3)); // 5^3
            Debug.Log(Math.Log(8, 2)); // log2(8)
            Debug.Log(Math.Exp(5)); // e^5
        */
        }

        public void Update()
        {
            var dir = UnityEngine.Input.GetAxisRaw("Horizontal");
            if (UnityEngine.Input.GetButtonDown("Horizontal"))
            {
                _first = true;
                _unityRb2D.velocity = Vector2.zero;
                _unityRb2D.AddForce(new Vector2(dir * force, 0), ForceMode2D.Impulse);
                StartCoroutine(Coroutine());
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                _unityRb2D.velocity = Vector2.zero;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Recode());
            }
        }

        private void AddForce(int dir)
        {
            _unityRb2D.velocity = Vector2.zero;
            _unityRb2D.AddForce(new Vector2(dir * force, 0), ForceMode2D.Impulse);
            StartCoroutine(Coroutine());
        }

        private float Drag
        {
            get => _unityRb2D.drag;
            set
            {
                _unityRb2D.drag = value;
                if (value < 10) force = 10;
                else if (value < 100) force = 100;
                else if (value < 1000) force = 1000;
                else if (value < 10000) force = 10000;
            }
        }

        private IEnumerator Recode()
        {
            var dir = 1;
            while (_unityRb2D.drag < 10000)
            {
                while (Drag <= 10)
                {
                    Drag += 1;
                    AddForce(dir);
                    dir = -dir;
                    yield return new WaitForSeconds(0.5f);
                }

                while (Drag <= 100)
                {
                    Drag += 10;
                    AddForce(dir);
                    dir = -dir;
                    yield return new WaitForSeconds(0.5f);
                }

                while (Drag <= 1000)
                {
                    Drag += 50;
                    AddForce(dir);
                    dir = -dir;
                    yield return new WaitForSeconds(0.5f);
                    Drag += 1;
                    AddForce(dir);
                    dir = -dir;
                    yield return new WaitForSeconds(0.5f);
                    Drag += 2;
                    AddForce(dir);
                    dir = -dir;
                    yield return new WaitForSeconds(0.5f);
                    Drag -= 4;
                    AddForce(dir);
                    dir = -dir;
                    yield return new WaitForSeconds(0.5f);
                    Drag += 3;
                }

                Drag += 100;
                AddForce(dir);
                dir = -dir;
                yield return new WaitForSeconds(0.5f);
                Drag += 1;
                AddForce(dir);
                dir = -dir;
                yield return new WaitForSeconds(0.5f);
                Drag += 2;
                AddForce(dir);
                dir = -dir;
                yield return new WaitForSeconds(0.5f);
                Drag -= 4;
                AddForce(dir);
                dir = -dir;
                yield return new WaitForSeconds(0.5f);
                Drag += 3;
            }
        }

        private IEnumerator Coroutine()
        {
            var list = new List<double>();
            v0 = Math.Abs(_unityRb2D.velocity.x);
            drag = _unityRb2D.drag;
            list.Add(v0);
            list.Add(drag);
            while (list.Count < 6)
            {
                list.Add(Math.Abs(_unityRb2D.velocity.x));
                Debug.Log(Math.Abs(_unityRb2D.velocity.x));
                yield return new WaitForFixedUpdate();
            }

            var queue = new Queue<string>();
            if (Math.Abs(list[3] / list[2] - list[4] / list[3]) < 0.001)
            {
                queue.Enqueue(v0.ToString());
                queue.Enqueue(drag.ToString());
                queue.Enqueue((list[3] / list[2]).ToString());
                var fileName = $"drag {drag}.txt";
                Debug.Log(fileName);
                Save(string.Join(" ", queue.ToArray()), fileName);
            }
        }

        private int _directoryCode = 0;
        private string directory = "/CustomData/";
        private string FileName => $"data{_directoryCode}.txt";

        private void Save(string contents, string fileName)
        {
            var dir = Application.persistentDataPath + directory;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(dir + fileName, contents);
            Debug.Log(dir);
        }

        private void Save(string contents)
        {
            var dir = Application.persistentDataPath + directory;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            while (File.Exists(dir + FileName)) _directoryCode++;

            File.WriteAllText(dir + FileName, contents);
            Debug.Log(dir);
            _directoryCode++;
        }
    }
}