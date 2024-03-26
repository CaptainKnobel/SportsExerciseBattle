using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEB.Services;

namespace SEB.Controllers
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