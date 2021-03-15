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
        readonly ILabelManagementBL labelManagementBL;

        public LabelsController(ILabelManagementBL labelManagementBL)
        {
            this.labelManagementBL = labelManagementBL;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUserLabels()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    ICollection<ResponseLabel> result = labelManagementBL.GetUserLabels(UserID);
                    return Ok(new { success = true, user = Email, Labels = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.InnerException });
            }
        }

        [Authorize]
        [HttpDelete("Delete/{LabelID}")]
        public IActionResult DeleteUserLabel(long LabelID)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    bool result = labelManagementBL.DeleteUserLabel(UserID, LabelID);
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
