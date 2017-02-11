using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.UI.AttackWay;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class CompleteStep : NetworkBehaviour
    {
        private Client _client;

        private Color32 _colorSelect;

        private Color32 _colorOrigin;

        private Image _image;


        void Start()
        {
            if (!localPlayerAuthority) return;
            _client = GameObjectHelper.FindGamerClient();

            _image = GetComponent<Image>();
            _colorOrigin = _image.color;
            _colorSelect = new Color32(225, 225, 225, 255);
        }

        void OnMouseDown()
        {
            if (!localPlayerAuthority) return;
            _client.CompleteStep();
        }

        void OnMouseEnter()
        {
            _image.color = _colorSelect;
        }

        void OnMouseExit()
        {
            _image.color = _colorOrigin;
        }
    }
}
