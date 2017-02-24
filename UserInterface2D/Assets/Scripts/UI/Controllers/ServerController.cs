using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI.Cards;
using Assets.Scripts.UI.Game;
using Domain.Cards;
using Domain.Coins;
using Domain.GameProcess;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Controllers
{
    public class ServerController : NetworkBehaviour
    {
        private GameUnity game;
        void Start()
        {
            game = GetComponent<GameScript>().GetGame();
            SubscribeToEvents();
        }
        void SubscribeToEvents()
        {
            game.onOpenAll += OnOpenAll;           
            game.onCloseCoins += OnCloseCoins;
            game.onUpdateCountCoins += OnUpdateCountCoins;
            game.onCreateActiveCard += OnCreateActiveCard;            
            game.onChangeHealthCard += OnChangeHealthCard;
            game.onChangeClosedCard += OnChangeClosedCard;
            game.onDestroyCardInHand += OnDestroyCardInHand;
            game.onUpdateCardsInHand += OnUpdateCardsInHand;
            game.onSelectionAttackWay += OnSelectionAttackWay;
        }
        #region event ConnectPlayer
        public void CmdConnectPlayers(NetworkInstanceId networkId)
        {
            if (!isServer) return;

            GetComponent<GameScript>().ConnectPlayer(networkId);
        }

        public void PlayerInitialization(Player player, NetworkInstanceId networkId)
            => GetClient(networkId).CmdPlayerInitialization(player.Id.ToString(), 
                GetHeroInfo(player), player.ActiveDeck.Select(x => 
                new CardInfo() { InstId = x.InstId.ToString(), CardId = x.CardId }).ToArray(), player.Money.Count);

        public void EnemyInitialization(Player enemy, NetworkInstanceId networkId)
            => GetClient(networkId).CmdEnemyInitialization(enemy.Id.ToString(), 
                GetHeroInfo(enemy), enemy.ActiveDeck.Count(), enemy.Money.Count);

        private CardInfo GetHeroInfo(Player player)
            => new CardInfo() { InstId = player.Hero.InstId.ToString(), CardId = player.Hero.CardId, Health = player.Hero.Health };
        #endregion

        #region event onStartStep
        private void OnOpenAll(Guid playerId)
            => GetClients().ForEach(x => x.CmdonOpenAll(playerId.ToString()));

        private void OnUpdateCountCoins(Guid playerId, int countCoinsPlayer)
            =>GetClients().ForEach(x => x.CmdUpdateCountCoins(playerId.ToString(), countCoinsPlayer));

        private void OnUpdateCardsInHand(Guid playerId, List<Card> cards)
            => GetClients().ForEach(x=>x.CmdUpdateCardsInHand(playerId.ToString(), cards.Select(card =>
                new CardInfo() { InstId = card.InstId.ToString(), CardId = card.CardId }).ToArray()));
        #endregion

        #region event SelectCardInHand
        public void CmdHire(string playerId, string instId)
        {
            if (!isServer) return;
            game.HireEntity(new Guid(playerId), new Guid(instId));
        }
        private void OnCreateActiveCard(Card card, Guid playerId)
            => GetClients().ForEach(x => x.CmdCreateActiveCard(new CardInfo()
            { InstId = card.InstId.ToString(), CardId = card.CardId,
                Power = card.Power, Health = card.Health}, playerId.ToString()));

        private void OnCloseCoins(int count, Guid playerId)
            => GetClients().ForEach(x => x.CmdCloseCoins(count, playerId.ToString()));

        private void OnDestroyCardInHand(Guid instId, Guid playerId)
            => GetClients().ForEach(x => x.CmdDestroyCardInHand(instId.ToString(), playerId.ToString()));
        #endregion

        #region event ChangeHealth,ChangeClosed and Select ActiveCard
        private void OnChangeHealthCard(Guid instId, int health) 
            => GetClients().ForEach(x=>x.CmdonChangeHealthCard(instId.ToString(), health));

        private void OnChangeClosedCard(Guid instId, bool closed) 
            => GetClients().ForEach(x => x.CmdonChangeClosedCard(instId.ToString(), closed));   

        //переименовать
        public void CmdSaveSelectActiveCards(string playerId, string instId, NetworkInstanceId networkId)
        {
            if (!isServer) return;
            game.SaveSelectActiveCards(new Guid(playerId), new Guid(instId), networkId);
        }
        #endregion

        #region SetAttackWay
        private void OnSelectionAttackWay(NetworkInstanceId networkId) => GetClient(networkId).CmdActiveAttackWayPanel();
        public void CmdSetAttackWay(CardActionEnum attackWay) => game.SetAttackWay(attackWay);
        #endregion

        #region CompleteStep
        public void CmdCompleteStep(string playerId) => game.CompleteStep(new Guid(playerId));
        #endregion

        private ClientController GetClient(NetworkInstanceId networkId) => GameObject.FindGameObjectsWithTag("Gamer")
            .Select(x=>x.GetComponent<ClientController>()).FirstOrDefault(x => x.netId == networkId);
        private List<ClientController> GetClients() => GameObject.FindGameObjectsWithTag("Gamer")
            .Select(x => x.GetComponent<ClientController>()).ToList();
    }
}

