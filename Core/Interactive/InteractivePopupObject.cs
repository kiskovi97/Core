using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class InteractivePopupObject : MonoBehaviour
    {
        public AnimatedObject infoObj;
        public GameObject interactableVisual;
        public TriggerAction OnInteract;

        private HashSet<GameObject> players = new HashSet<GameObject>();

        [Inject] private SignalBus signalBus;

        private bool _isInteractable = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                players.Add(other.gameObject);
                CheckAll();
            }
        }

        private void Subscribe()
        {
            signalBus.Subscribe<UIInteractions.AcceptSignal>(OnInterract);
        }

        private void Unsubscribe()
        {
            signalBus.TryUnsubscribe<UIInteractions.AcceptSignal>(OnInterract);
        }

        private void CheckAll(bool force = false)
        {
            var isInteractable = players.Any();
            if (_isInteractable != isInteractable || force)
            {
                _isInteractable = isInteractable;

                if (infoObj != null)
                {
                    infoObj.SetActive(false, force);
                }

                if (isInteractable)
                    Subscribe();
                else
                    Unsubscribe();
            }

            if (interactableVisual != null)
                interactableVisual.SetActive(_isInteractable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                players.Remove(other.gameObject);
                CheckAll();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                players.Add(collision.gameObject);
                CheckAll();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                players.Remove(other.gameObject);
                CheckAll();
            }
        }

        protected void Awake()
        {
            players.Clear();

            CheckAll(true);
        }

        protected void OnDisable()
        {
            players.Clear();

            CheckAll();
        }

        protected virtual void OnInterract()
        {
            TriggerAction.Trigger(OnInteract);

            if (infoObj != null)
            {
                infoObj.SetActive(!infoObj.PrevValue);
            }
        }
    }
}
