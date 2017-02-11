using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Application.Cards;
using Assets.Scripts.Application.Game;
using Domain.Cards;
using Domain.Process;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Infrastructure
{
    public class ServerGame : NetworkBehaviour
    {
        GameScript game;
        void Start()
        {
            game = GetComponent<GameScript>();
        }

        public void CmdConnectPlayers(NetworkInstanceId networkId)
        {
            if (!isServer) return;

            GameState state = game.GetGameState();
            Player player = GameObject.FindWithTag("Scripts").GetComponent<GameScript>().GetPlayer();
            Player enemy = (state.MovingPlayer.Id == player.Id) ? state.WaitingPlayer : state.MovingPlayer;

            Client client = GetClient(networkId);
            client.CmdPlayerInitialization(player.Id.ToString(), GetHeroInfo(player), player.ActiveDeck.Select(x =>
                new CardInfo() { InstId = x.InstId.ToString(), CardId = x.CardId }).ToArray(), player.Money.Count);
            client.CmdEnemyInitialization(enemy.Id.ToString(), GetHeroInfo(enemy), enemy.ActiveDeck.Count(), enemy.Money.Count);
        }
        private CardInfo GetHeroInfo(Player player)
            => new CardInfo() { InstId = player.Hero.InstId.ToString(), CardId = player.Hero.CardId, Health = player.Hero.Health };

        #region event SrartStep
        public void CmdSubscribeToSrartStep()
        {
            if (!isServer) return;
            game.OnStartStep += CmdonStartStep;
        }
        void CmdonStartStep(string playerId)
        {
            var state = game.GetGameState();
            var movingplayer = state.MovingPlayer;
            var watingplayer = state.WaitingPlayer;
            GetClients().ForEach(x =>
            {
                x.CmdonStartStep(playerId);
                x.CmdUpdateCountCoins(
                    new string[] { movingplayer.Id.ToString(), watingplayer.Id.ToString() },
                    new int[] { movingplayer.Money.Count, watingplayer.Money.Count });
            });
        }
        #endregion

        #region event AddCoin
        public void CmdSubscribeToAddCoin(string playerId)
        {
            if (!isServer) return;
            GameState state = game.GetGameState();
            if (state.MovingPlayer.Id.ToString() == playerId)
                SubscribePlayersToAddCoin(state.MovingPlayer, state.WaitingPlayer);
            else
                SubscribePlayersToAddCoin(state.WaitingPlayer, state.MovingPlayer);
        }
        private void SubscribePlayersToAddCoin(Player playerGamer, Player playerEnemy)
        {
            SubscribePlayerToAddCoin(playerGamer, CmdonAddCoin);
            SubscribePlayerToAddCoin(playerEnemy, CmdonAddEnemyCoin);
        }
        private void SubscribePlayerToAddCoin(Player player, Money.OnAddCoinHandler method)
            => player.Money.onAddCoin += method;
        void CmdonAddCoin() => GetClients().ForEach(x => x.CmdonAddCoin());
        void CmdonAddEnemyCoin() => GetClients().ForEach(x => x.CmdonAddEnemyCoin());
        #endregion

        #region event SelectCardInHand
        public void CmdHire(string playerId, string instId)
        {
            if (!isServer) return;
            Player movingPlayer = game.GetGameState().MovingPlayer;
            if (playerId != movingPlayer.Id.ToString())
                return;
            bool result = movingPlayer.Hire(new Guid(instId));
            if (!result) return;
            Card card = movingPlayer.CardsInGame.FirstOrDefault(x => x.InstId.ToString() == instId);
            GetClients().ForEach(x=> {
                x.CmdCreateActiveCard(new CardInfo() { InstId = card.InstId.ToString(), CardId = card.CardId,
                    Power = card.Power, Health = card.Health }, playerId);
                x.CmdCloseCoins(card.Cost, playerId);
                x.CmdDestroyCardInHand(instId,playerId);
            });
        }  
        #endregion

        #region event ChangeHealth,ChangeClosed and Select ActiveCard
        public void CmdSubscribeToActiveCard(string instId)
        {
            if (!isServer) return;
            GameState state = game.GetGameState();
            Card card = FindCard(state.MovingPlayer, instId);
            card = card ?? FindCard(state.WaitingPlayer, instId);
            card.onChangeHealth += CmdonChangeHealth;
            card.onChangeClosed += CmdonChangeClosed;
        }
        Card FindCard(Player player, string instId)
            => (player.Hero.InstId.ToString() == instId) ? player.Hero : 
                player.CardsInGame.FirstOrDefault(x => x.InstId.ToString() == instId);
        void CmdonChangeHealth(Guid instId, int health) => GetClients().ForEach(x=>x.CmdonChangeHealth(instId.ToString(), health));
        void CmdonChangeClosed(Guid instId, bool closed) => GetClients().ForEach(x => x.CmdonChangeClosed(instId.ToString(), closed));   
        public void CmdSaveActiveCards(string playerId, string instId, NetworkInstanceId networkId)
        {
            if (!isServer) return;
            Player movingPlayer = game.GetGameState().MovingPlayer;
            if (playerId != movingPlayer.Id.ToString()) return;

            if (game.ActionCardId == "")
                game.ActionCardId = instId;
            else if (game.ActionCardId != instId)
            {
                game.TargetCardId = instId;
                GetClient(networkId).CmdActiveAttackWayPanel();
            }
            else
                game.SetToZeroCards();
        }
        #endregion

        #region SetAttackWay
        public void CmdSetAttackWay(CardActionEnum attackWay) => game.AttackWay = attackWay;
        #endregion

        #region CompleteStep
        public void CmdCompleteStep(string playerId)
        {
            game.CompleteStep(playerId);
        }
        #endregion

        private Client GetClient(NetworkInstanceId networkId) => GameObject.FindGameObjectsWithTag("Gamer")
            .Select(x=>x.GetComponent<Client>()).FirstOrDefault(x => x.netId == networkId);
        private List<Client> GetClients() => GameObject.FindGameObjectsWithTag("Gamer")
            .Select(x => x.GetComponent<Client>()).ToList();
    }
}

