using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Extensions
{
    public static class UserExtensions
    {
        public static async Task<User> GetUser(this ClaimsPrincipal user, ZdcContext context)
        {
            var cid = user.Claims
                .FirstOrDefault(x => x.Type.Equals("cid", StringComparison.OrdinalIgnoreCase))?
                .Value;
            if (cid == null)
                return null;
            var controller = await context.Users.FindAsync(cid);
            return controller;
        }

        public static async Task<bool> IsStaff(this ClaimsPrincipal user, ZdcContext context)
        {
            var cid = user.Claims
                .FirstOrDefault(x => x.Type.Equals("cid", StringComparison.OrdinalIgnoreCase))?
                .Value;
            if (cid == null)
                return false;
            var controller = await context.Users.FindAsync(cid);
            if (controller == null)
                return false;
            var staff = controller.Roles
                .Any(x => x.Name.Equals("ATM") || x.Name.Equals("DATM") || x.Name.Equals("TA")
                          || x.Name.Equals("WM") || x.Name.Equals("EC") || x.Name.Equals("FE")
                          || x.Name.Equals("ATA") || x.Name.Equals("AWM") || x.Name.Equals("AEC")
                          || x.Name.Equals("AFE"));
            return staff;
        }

        public static async Task<bool> IsTrainingStaff(this ClaimsPrincipal user, ZdcContext context)
        {
            var cid = user.Claims
                .FirstOrDefault(x => x.Type.Equals("cid", StringComparison.OrdinalIgnoreCase))?
                .Value;
            if (cid == null)
                return false;
            var controller = await context.Users.FindAsync(cid);
            if (controller == null)
                return false;
            var training = controller.Roles
                .Any(x => x.Name.Equals("INS") || x.Name.Equals("MTR"));
            return training;
        }

        public static async Task<bool> HasEventRegistration(this ClaimsPrincipal user, ZdcContext context, int id)
        {
            var cid = user.Claims
                .FirstOrDefault(x => x.Type.Equals("cid", StringComparison.OrdinalIgnoreCase))?
                .Value;
            if (cid == null)
                return false;
            var controller = await context.Users.FindAsync(cid);
            var registration = await context.EventRegistrations.FindAsync(id);
            if (controller == null || registration == null)
                return false;
            return registration.User == controller;
        }
    }
}