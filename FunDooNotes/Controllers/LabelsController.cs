using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using LabelInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunDooNotes.Controllers
{
    /// <summary>
    /// Manage user note labels
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiController]
    [Route("[controller]")]
    public class LabelsController : Controller
    {
        readonly ILabelManagementBL labelManagementBL;

        public LabelsController(ILabelManagementBL labelManagementBL)
        {
            this.labelManagementBL = labelManagementBL;
        }

        /// <summary>
        /// Gets the user labels.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult GetUserLabels()
        {
            try
            {
                if (User.Identity is ClaimsIdentity identity)
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
        /// <summary>
        /// Deletes the user label.
        /// </summary>
        /// <param name="LabelID">The label identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{LabelID}")]
        public IActionResult DeleteUserLabel(long LabelID)
        {
            try
            {
                if (User.Identity is ClaimsIdentity identity)
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
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Changes the name of the label.
        /// </summary>
        /// <param name="LabelID">The label identifier.</param>
        /// <param name="LabelName">Name of the label.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Edit/{LabelID}/{LabelName}")]
        public IActionResult ChangeLabelName(long LabelID, string LabelName)
        {
            try
            {
                if (User.Identity is ClaimsIdentity identity)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    bool result = labelManagementBL.ChangeLabelName(UserID, LabelID, LabelName);
                    return Ok(new { success = true, user = Email, LabelChanged = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Adds the new user label.
        /// </summary>
        /// <param name="LabelName">Name of the label.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add/{LabelName}")]
        public IActionResult AddUserLabel(string LabelName)
        {
            try
            {
                if (User.Identity is ClaimsIdentity identity)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    bool result = labelManagementBL.AddUserLabel(UserID, LabelName);
                    return Ok(new { success = true, user = Email, LabelAdded = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Gets the label notes.
        /// </summary>
        /// <param name="LabelName">Name of the label.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{LabelName}")]
        public IActionResult GetLabelNotes(string LabelName)
        {
            try
            {
                if (User.Identity is ClaimsIdentity identity)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;
                    ICollection<ResponseNoteModel> result = labelManagementBL.GetLabelNotes(UserID, LabelName);
                  
                    return Ok(new { success = true, user = Email, notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
    }
}
