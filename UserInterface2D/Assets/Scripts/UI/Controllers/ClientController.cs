using Assets.Scripts.UI.Cards;
using Assets.Scripts.UI.Players;
using Domain.Cards;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Controllers
{
    public class ClientController : NetworkBehaviour
    {
        public GameObject AttackWayPanel;
        public GameObject EnemyPrefab;
        private Enemy _enemy;
        private Gamer _gamer;
        public override void OnStartLocalPlayer()
        {
            if (!isLocalPlayer) return;
            _gamer = GetComponent<Gamer>();
            CreateEnemy();           
            CmdConnectPlayer(netId);
        }
        private void CreateEnemy()
        {
            GameObject enemy = Instantiate(EnemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _enemy = enemy.GetComponent<Enemy>();
        }

        #region ConnectPlayers
        [Command]
        void CmdConnectPlayer(NetworkInstanceId networkId) => GetServer().CmdConnectPlayers(networkId);
        public void CmdEnemyInitialization(string playerId, CardInfo heroInfo, int countCards, int countCoin)
            => RpcEnemyInitialization(playerId, heroInfo, countCards, countCoin);
        public void CmdPlayerInitialization(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
            => RpcPlayerInitialization(playerId, heroInfo, cardsInfo, countCoin);
        [ClientRpc]
        void RpcEnemyInitialization(string playerId, CardInfo heroInfo, int countCards, int countCoin)
        {
            if (!isLocalPlayer) return;
            _enemy?.OnStartPlayer(playerId, heroInfo, new CardInfo[0], countCoin);
        }
        [ClientRpc]
        void RpcPlayerInitialization(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
        {
            if (!isLocalPlayer) return;
            _gamer.OnStartPlayer(playerId, heroInfo, cardsInfo, countCoin);
        }
        #endregion

        #region event SrartStep
        public void CmdonOpenAll(string playerId) => RpconOpenAll(playerId);

        public void CmdUpdateCountCoins(string playerId, int countCoinsPlayers)
            => RpcUpdateCountCoins(playerId, countCoinsPlayers);

        public void CmdUpdateCardsInHand(string playerId, CardInfo[] cardsInfo) => RpcUpdateCardsInHand(playerId, cardsInfo);

        [ClientRpc]
        void RpconOpenAll(string playerId)
        {
            if (!isLocalPlayer) return;
            _gamer.OpenAll(playerId);
            _enemy.OpenAll(playerId);
        }

        [ClientRpc]
        void RpcUpdateCountCoins(string playerId, int countCoinsPlayer)
        {
            if (!isLocalPlayer) return;
            _gamer.UpdateCountCoins(playerId, countCoinsPlayer);
            _enemy.UpdateCountCoins(playerId, countCoinsPlayer);
        }

        [ClientRpc]
        void RpcUpdateCardsInHand(string playerId, CardInfo[] cardsInfo)
        {
            if (!isLocalPlayer) return;
            _gamer.UpdateCardsInHand(playerId, cardsInfo);
        }
        #endregion    

        #region event SelectCardInHand
        public void SubscribeToSelectCardInHand(GameObject sprite)
            => sprite.GetComponent<CardInHand>().OnSelectCard += onSelectCard;
        void onSelectCard(string instId)
        {
            if (!isLocalPlayer) return;
            CmdHire(_gamer.GetId(), instId);
        }
        [Command]
        void CmdHire(string playerId, string instId) => GetServer().CmdHire(playerId, instId);
        public void CmdCreateActiveCard(CardInfo cardInfo, string playerId) => RpcCreateActiveCard(cardInfo, playerId);
        public void CmdCloseCoins(int count, string playerId) => RpcCloseCoins(count, playerId);
        public void CmdDestroyCardInHand(string instId, string playerId) => RpcDestroyCardInHand(instId, playerId);
        [ClientRpc]
        void RpcCreateActiveCard(CardInfo cardInfo, string playerId)
        {
            if (!isLocalPlayer) return;
            _gamer.CreateActiveCard(cardInfo, playerId);
            _enemy.CreateActiveCard(cardInfo, playerId);
        }
        [ClientRpc]
        void RpcCloseCoins(int count, string playerId)
        {
            if (!isLocalPlayer) return;
            _gamer.CloseCoins(count, playerId);
            _enemy.CloseCoins(count, playerId);
        }
        [ClientRpc]
        void RpcDestroyCardInHand(string instId, string playerId)
        {
            if (!isLocalPlayer) return;
            _gamer.DestroyCardInHand(instId, playerId);
            _enemy.DestroyCardInHand(instId, playerId);
        }
        #endregion

        #region event ChangeHealth,ChangeClosed and Select ActiveCard
        public void SubscribeToEventsHero(GameObject sprite)
        {
            Hero hero = sprite.GetComponent<Hero>();
            hero.OnSelectCard += onSelectActiveCard;
        }
        public void SubscribeToEventsActiveCard(GameObject sprite) 
        {
            CardUnity card = sprite.GetComponent<CardUnity>();
            card.OnSelectCard += onSelectActiveCard;
        }
        bool onSelectActiveCard(string instId)
        {
            if (!isLocalPlayer) return false;
            CmdSaveActiveCards(_gamer.GetId(), instId,netId);
            return true;
        }
        [Command]
        void CmdSaveActiveCards(string playerId, string instId, NetworkInstanceId networkId)
            => GetServer().CmdSaveSelectActiveCards(playerId, instId, networkId);
        public void CmdonChangeHealthCard(string instId, int health) => RpconChangeHealthCard(instId, health);
        public void CmdonChangeClosedCard(string instId, bool closed) => RpconChangeClosedCard(instId, closed);
        [ClientRpc]
        void RpconChangeHealthCard(string instId, int health)
        {
            if (!isLocalPlayer) return;
            _gamer.OnChangeHealthCard(instId, health);
            _enemy.OnChangeHealthCard(instId, health);
        }
        [ClientRpc]
        void RpconChangeClosedCard(string instId, bool closed)
        {
            if (!isLocalPlayer) return;
            _gamer.OnChangeClosedCard(instId, closed);
            _enemy.OnChangeClosedCard(instId, closed);
        }
        #endregion

        #region SetAttackWay
        public void CmdActiveAttackWayPanel() => RpcActiveAttackWayPanel();
        [ClientRpc]
        void RpcActiveAttackWayPanel()
        {
            if (!isLocalPlayer) return;

            AttackWayPanel.SetActive(true);
        }
        public void SetAttackWay(CardActionEnum attackWay)
        {
            AttackWayPanel.SetActive(false);
            CmdSetAttackWay(attackWay);
        }
        [Command]
        void CmdSetAttackWay(CardActionEnum attackWay) => GetServer().CmdSetAttackWay(attackWay);
        #endregion

        #region CompleteStep
        public void CompleteStep() => CmdCompleteStep(_gamer.GetId());
        [Command]
        void CmdCompleteStep(string playerId) => GetServer().CmdCompleteStep(playerId);        
        #endregion

        private ServerController GetServer() => GameObject.FindWithTag("Scripts").GetComponent<ServerController>();
    }
}

