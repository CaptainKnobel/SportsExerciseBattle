using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.BusinessLayer;

namespace SportsExerciseBattle.BusinessLayer.Menus
{
    public class ProfileController
    {
        private readonly UserService userService;

        public ProfileController(UserService userService)
        {
            this.userService = userService;
        }

        public void Run()
        {
            // TODO: Implement profile (+ menu options)
        }
    }
}