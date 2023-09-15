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
        public ElementPoints ElementPoints { get; set; }
        public int SpellsCount { get; set; }
        public SpellSlots Spells { get; set; }
        public CompanionSlots Companions { get; set; }
        public int CompanionsCount { get; set; }

        public PlayerInfos() {}

        public void Update()
        {
            if (PlayerData.initialized)
            {
                AccountName = PlayerData.instance.nickName.Nickname;
                HeroName = PlayerData.instance.heroCollectionData.currentPlayerHero.name;
                Class = PlayerData.instance.heroCollectionData.currentPlayerHero.god.ToString();
                Weapon = PlayerData.instance.heroCollectionData.currentPlayerHero.weaponDefinition.displayName.Split
                    (
                        PlayerData.instance.heroCollectionData.currentPlayerHero.god.ToString().ToUpper() + " - "
                    )[1];
                Level = PlayerData.instance.heroCollectionData.currentPlayerHero.level;
                if (GameStatus.isInFight)
                {
                    IsInFight = true;
                    IsPlayerTurn = FightStatus.local.currentTurnPlayerId == FightStatus.local.GetLocalPlayer().id;
                    PlayerStatus player = FightStatus.local.GetLocalPlayer();
                    BaseHealthPoints = player.heroStatus.baseLife;
                    HealthPoints = player.heroStatus.life;
                    ActionPoints = player.actionPoints;
                    ReservePoints = player.reservePoints;
                    BaseMovementPoints = player.heroStatus.baseMovementPoints;
                    MovementPoints = player.heroStatus.movementPoints;
                    SpellsCount = FightStatus.local.GetLocalPlayer().m_spells.Count;
                    Spells = new SpellSlots();
                    var spells = new List<Spell>();
                    foreach (var spell in FightStatus.local.GetLocalPlayer().m_spells)
                    {
                        Spell newSpell = new Spell();
                        newSpell.Name = spell.Value.spellDefinition.displayName.Split(" - ")[3];
                        newSpell.Element = spell.Value.spellDefinition.details.element.ToString();
                        newSpell.Cost = spell.Value.baseCost.Value;
                        spells.Add(newSpell);
                    }
                    if (SpellsCount >= 1)
                        Spells.Slot1 = spells[0];
                    if (SpellsCount >= 2)
                        Spells.Slot2 = spells[1];
                    if (SpellsCount >= 3)
                        Spells.Slot3 = spells[2];
                    if (SpellsCount >= 4)
                        Spells.Slot4 = spells[3];
                    if (SpellsCount >= 5)
                        Spells.Slot5 = spells[4];
                    if (SpellsCount >= 6)
                        Spells.Slot6 = spells[5];
                    if (SpellsCount >= 7)
                        Spells.Slot7 = spells[6];
                    Companions = new CompanionSlots();
                    CompanionsCount = player.m_availableCompanions.Count;
                    PropertyInfo[] rootProperties = typeof(CompanionSlots).GetProperties();
                    for (var i = 0; i < CompanionsCount; i++)
                    {
                        Companion newCompanion = new Companion();
                        newCompanion.Name = player.m_availableCompanions[i].definition.displayName;
                        newCompanion.Element = player.m_availableCompanions[i].definition.GetElement().Value.ToString();
                        newCompanion.State = player.m_availableCompanions[i].state.ToString();
                        rootProperties[i].SetValue(Companions, newCompanion);
                    }
                    ElementPoints = new ElementPoints();
                    //ElementPoints.FirePoints = player.heroStatus.GetCarac(CaracId.FirePoints);
                    //ElementPoints.WaterPoints = player.heroStatus.GetCarac(CaracId.WaterPoints);
                    //ElementPoints.EarthPoints = player.heroStatus.GetCarac(CaracId.EarthPoints);
                    //ElementPoints.AirPoints = player.heroStatus.GetCarac(CaracId.AirPoints);
                }
                else
                {
                    IsInFight = false;
                }
            }
        }
    }
}

