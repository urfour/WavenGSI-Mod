using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Player;
using Ankama.Cube.UI.Fight;
using Channel;
using System.Collections.Generic;
using System.Reflection;
using WavenGSI.Player;

namespace WavenGSI
{
    public class PlayerInfos
    {
        public string AccountName { get; set; }
        public string HeroName { get; set; }
        public string Class { get; set; }
        public string Weapon { get; set; }
        public int Level { get; set; }
        public bool IsInFight { get; set; }
        public bool IsPlayerTurn { get; set; }
        public int BaseHealthPoints { get; set; }
        public int HealthPoints { get; set; }
        public int ActionPoints { get; set; }
        public int ReservePoints { get; set; }
        public int BaseMovementPoints { get; set; }
        public int MovementPoints { get; set; }
        public int SpellsCount { get; set; }
        public SpellSlots Spells { get; set; }
        public CompanionSlots Companions { get; set; }
        public int CompanionsCount { get; set; }

        public PlayerInfos() { }

        public void Update()
        {
            if (PlayerData.initialized)
            {
                AccountName = PlayerData.instance.nickName.Nickname;
                var currentHero = PlayerData.instance.heroCollectionData.currentPlayerHero;
                HeroName = currentHero.name;
                Class = currentHero.god.ToString();
                Weapon = currentHero.weaponDefinition.displayName.Split(Class.ToUpper() + " - ")[1];
                Level = currentHero.level;
                if (GameStatus.isInFight)
                {
                    if (FightStatus.local != null)
                    {
                        PlayerStatus player = FightStatus.local.GetLocalPlayer();
                        LocalPlayerHUD playerHUD = FightUI.instance.GetLocalPlayerHUD();
                        if (player != null)
                        {
                            IsInFight = true;
                            IsPlayerTurn = FightStatus.local.currentTurnPlayerId == player.id;
                            BaseHealthPoints = player.heroStatus.baseLife;
                            HealthPoints = player.heroStatus.life;
                            ActionPoints = player.actionPoints;
                            ReservePoints = player.reservePoints;
                            BaseMovementPoints = player.heroStatus.baseMovementPoints;
                            MovementPoints = player.heroStatus.movementPoints;
                            Spells = new SpellSlots();
                            PropertyInfo[] spellProperties = typeof(SpellSlots).GetProperties();
                            int spellCount = 0;
                            foreach (var spell in player.m_spells.Values)
                            {
                                if (spell.location == SpellMovementZone.Hand)
                                {
                                    Spell newSpell = new Spell
                                    {
                                        Name = spell.spellDefinition.displayName,
                                        Element = spell.spellDefinition.details.element.ToString(),
                                        Cost = spell.baseCost.Value,
                                        IsAvailable = IsPlayerTurn && spell.baseCost.Value <= ActionPoints
                                    };
                                    spellProperties[spellCount].SetValue(Spells, newSpell);
                                    spellCount++;
                                }
                            }
                            SpellsCount = spellCount;
                            for (; spellCount < SpellsCount; spellCount++)
                            {
                                spellProperties[spellCount].SetValue(Spells, null);
                            }
                            Companions = new CompanionSlots();
                            CompanionsCount = player.m_availableCompanions.Count;
                            PropertyInfo[] companionProperties = typeof(CompanionSlots).GetProperties();
                            for (var i = 0; i < CompanionsCount; i++)
                            {
                                Companion newCompanion = new Companion
                                {
                                    Name = player.m_availableCompanions[i].definition.displayName,
                                    Element = player.m_availableCompanions[i].definition.GetElement().Value.ToString(),
                                    State = player.m_availableCompanions[i].state.ToString(),
                                    Rarity = player.m_availableCompanions[i].definition.rarity.ToString()
                                };
                                if (IsPlayerTurn && playerHUD.GetCompanionAtIndex(i) != null)
                                    newCompanion.IsAvailable = playerHUD.GetCompanionAtIndex(i).Value.canBeCast;
                                companionProperties[i].SetValue(Companions, newCompanion);
                            }
                        }
                    }
                }
                else
                {
                    IsInFight = false;
                }
            }
        }
    }
}