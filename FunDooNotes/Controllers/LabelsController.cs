using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLayer.ResponseModel;
using LabelInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunDooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabelsController : Controller
    {
        ILabelManagementBL labelManagementBL;

        public LabelsController(ILabelManagementBL labelManagementBL)
        {
            this.labelManagementBL = labelManagementBL;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetActiveNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    ICollection<Label> result = labelManagementBL.GetUserLabels(UserID);
                    return Ok(new { success = true, user = Email, Labels = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.InnerException });
            }
        }
    }
}
