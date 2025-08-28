using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets
{
    internal class PlayerHealth : Health
    {
        public override void Die()
        {
            SceneManager.LoadScene(0);
            GameManager.Instance.EndGame();
        }
    }
}
