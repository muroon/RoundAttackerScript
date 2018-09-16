using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace View.Scene
{
    [RequireComponent(typeof(Button))]
    public class ButtonSelectCharacter : MonoBehaviour
    {
        [SerializeField] int id;

        Button button;

        Subject<int> idSubject = new Subject<int>();
        public IObservable<int> IdEvent
        {
            get { return idSubject.AsObservable(); }
        }

        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AsObservable().Subscribe(_ => {
                idSubject.OnNext(id);
            });
        }
    }
}