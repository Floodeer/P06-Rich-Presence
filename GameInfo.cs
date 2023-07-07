using System.Collections.Generic;
using System.Linq;

namespace ProjectDR
{
    public class GameInfo
    {

        public string CurrentScene;
        public string CurrentStageImage;
        public bool MainMenu;

        public void Load()
        {
            CurrentScene = "Menu";
            CurrentStageImage = "gameimage";

            //Stages have multiple scenes
            m_StageScene = new Dictionary<string, List<string>>
            {
                { "Menu", new List<string>() {} },
                { "Wave Ocean", new List<string>() { "wvo_a_sn", "wvo_b_sd", "wvo_b_sn", "wvo_a_sd", "wvo_a_tl", "wvo_a_bz" } },
                { "Dust Desert", new List<string>() { "dtd_a_sd", "dtd_a_sn", "dtd_b_sd", "dtd_b_sv" } },
                { "Crisis City", new List<string>() { "csc_a_sn", "csc_b_sn", "csc_c_sn", "csc_a_sd", "csc_b_sd", "csc_c_sd", "csc_f1_sv", "csc_b_sv", "csc_e_sn", "csc_f_sd", "csc_f2_sv", "csc_f_sv" } },
                { "White Acropolis", new List<string>() { "wap_a_sn", "wap_a_sd", "wap_a_sv", "wap_b_sn", "wap_b_sd", "wap_b_sv" } },
                { "Flame Core", new List<string>() { "flc_a_sn", "flc_a_sd", "flc_a_sv", "flc_b_sn", "flc_b_sd", "flc_b_sv" } },
                { "Radical Train", new List<string>() { "rct_a_sn", "rct_a_sd", "rct_b_sn", "rct_b_sd", "rct_a_sv" } },
                { "Tropical Jungle", new List<string>() { "tpj_a_sn", "tpj_b_sn", "tpj_c_rg", "tpj_c_sv" } },
                { "Kingdom Valley", new List<string>() { "kdv_a_sn", "kdv_d_sn", "kdv_b_sn", "kdv_a_sd", "kdv_d_sd", "kdv_d_sv", "kdv_c_sn", "kdv_b_sd", "kdv_b_sv", "kdv_e_sn" } },
                { "Aquatic Base", new List<string>() { "aqa_a_sn", "aqa_a_sd", "aqa_a_sv", "aqa_b_sn", "aqa_b_sd", "aqa_b_sv" } },
                { "Test", new List<string>() { "test_b_sn" } }
            };

            /* The last two letters of the scene represents which characters is playing */
            m_CharacterNames = new Dictionary<string, string>()
            {
                { "sn", "Sonic" },
                { "bz", "Blaze" },
                { "tl", "Tails" },
                { "sd", "Shadow" },
                { "sv", "Silver" },
                { "rg", "Rouge" }
            };
        }

        /* Gets the current stage formatted by StageName - Character, also
           updates the image, removing spaces and putting to lower case */
        public string GetCurrentStageData(string scene)
        {
            foreach (var item in m_StageScene)
            {
                if (item.Value.Contains(scene))
                {
                    foreach (var characterCode in m_CharacterNames.Keys)
                    {
                        if (scene.IndexOf(characterCode) != -1)
                        {
                            updateImageData(item.Key, true);
                            return $"{item.Key} - {m_CharacterNames[characterCode]}"; //Stage - Character
                        }
                    }
                }
            }

            MainMenu = scene.SequenceEqual("MainMenu");

            //This works for the first stage loading screen and for scenes, it will not update the image until Main Menu is detected
            if (scene.Contains("Loading") && !MainMenu)
                return "Loading Section";

            updateImageData(scene, false);
            return "Menu";
        }

        private void updateImageData(string scene, bool stage)
        {
            CurrentStageImage = stage ? scene.ToLower().Replace(" ", "") : "gameimage";

        }

        private Dictionary<string, string> m_CharacterNames;
        private Dictionary<string, List<string>> m_StageScene;


    }
}
