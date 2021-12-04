using System.Collections.Generic;
using System.Linq;
using Examples.TankArena.Scripts.Data;
using Examples.TankArena.Scripts.Framework;
using Examples.TankArena.Scripts.Managers;
using Examples.TankArena.Scripts.SOReferences.GameReference;
using Examples.TankArena.Scripts.SOReferences.MatchReference;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.TankArena.Scripts.UI {
    public class TeamFightUI : MonoBehaviour {

        [Header("SO References")]
        public GameReference CurrentGameReference;
        public MatchReference CurrentMatchReference;

        [Header("Team A References")]
        public Toggle TeamACompositeToggle;
        public List<TMP_Dropdown> TeamADropdowns;
        
        [Header("Team B References")]
        public Toggle TeamBCompositeToggle;
        public List<TMP_Dropdown> TeamBDropdowns;
        
        [Header("Team C References")]
        public Toggle TeamCCompositeToggle;
        public List<TMP_Dropdown> TeamCDropdowns;
        
        [Header("Team D References")]
        public Toggle TeamDCompositeToggle;
        public List<TMP_Dropdown> TeamDDropdowns;

        private List<TankSetting> _tankSettings;

        private void Start() {
            _tankSettings = DataManager.TankSettings;
            InitDropDowns(TeamADropdowns, TeamACompositeToggle);
            InitDropDowns(TeamBDropdowns, TeamBCompositeToggle);
            InitDropDowns(TeamCDropdowns, TeamCCompositeToggle);
            InitDropDowns(TeamDDropdowns, TeamDCompositeToggle);
        }

        private void InitDropDowns(List<TMP_Dropdown> dropdowns, Toggle toggle) {
            foreach (TMP_Dropdown playerDropdown in dropdowns) {
                playerDropdown.ClearOptions();
                playerDropdown.AddOptions(new List<string> {"Empty"});
                playerDropdown.AddOptions(_tankSettings.Select(setting => setting.TankName + " of " + setting.PlayerName).ToList());
                playerDropdown.onValueChanged.AddListener(delegate(int value) {
                    if (!toggle.isOn) {
                        foreach (TMP_Dropdown dropdown in dropdowns) {
                            dropdown.SetValueWithoutNotify(value);
                        }
                    }
                });
                toggle.onValueChanged.AddListener(delegate(bool isComposite) {
                    for (int i = 1; i < dropdowns.Count; i++) {
                        dropdowns[i].interactable = isComposite;
                    }
                });
            }
        }

        public void CreateGame() {
            Game game = new Game();
            if (TeamADropdowns.Sum(dropdown => dropdown.value) > 0) {
                game.Teams.Add(new Team
                {
                    TankSettings = (from playerDropdown in TeamADropdowns 
                        where playerDropdown.value != 0 
                        select _tankSettings[playerDropdown.value - 1]).ToList(),
                    Color = Color.red
                });
            }
            if (TeamBDropdowns.Sum(dropdown => dropdown.value) > 0) {
                game.Teams.Add(new Team {
                    TankSettings = (from playerDropdown in TeamBDropdowns
                        where playerDropdown.value != 0
                        select _tankSettings[playerDropdown.value - 1]).ToList(),
                    Color = Color.green
                });
            }
            if (TeamCDropdowns.Sum(dropdown => dropdown.value) > 0) {
                game.Teams.Add(new Team {
                    TankSettings = (from playerDropdown in TeamCDropdowns
                        where playerDropdown.value != 0
                        select _tankSettings[playerDropdown.value - 1]).ToList(),
                    Color = Color.blue
                });
            }
            if (TeamDDropdowns.Sum(dropdown => dropdown.value) > 0) {
                game.Teams.Add(new Team {
                    TankSettings = (from playerDropdown in TeamDDropdowns
                        where playerDropdown.value != 0
                        select _tankSettings[playerDropdown.value - 1]).ToList(),
                    Color = Color.yellow
                });
            }
            game.SetupTeamFight();
            game.CurrentMatch = game.NextMatch();
            CurrentGameReference.Value = game;
            CurrentMatchReference.Value = game.CurrentMatch;
        }
        
    }
}
