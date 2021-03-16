using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.RedisCacheService;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using LabelInterfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryLayer.LabelInterfeces;

namespace BusinessLayer.LabelServices
{
    /// <summary>
    /// manages user's note Label 
    /// </summary>
    /// <seealso cref="LabelInterfaces.ILabelManagementBL" />
    public class LabelManagementBL : ILabelManagementBL
    {
        readonly ILabelManagementRL labelManagementRL;
        private readonly IDistributedCache distributedCache;
        readonly RedisCacheServiceBL redis;

        public LabelManagementBL(ILabelManagementRL labelManagementRL, IDistributedCache distributedCache)
        {
            this.labelManagementRL = labelManagementRL;
            this.distributedCache = distributedCache;
            redis = new RedisCacheServiceBL(this.distributedCache);
        }
        /// <summary>
        /// Adds the user label asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns></returns>
        public async Task<bool> AddUserLabelAsync(long userID, string labelName)
        {
            try
            {
                await redis.RemoveNotesRedisCache(userID);
                return labelManagementRL.AddNewUserLabel(userID, labelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool AddUserLabel(long userID, string labelName)
        {
            try
            {
                return AddUserLabelAsync(userID, labelName).Result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> ChangeLabelNameAsync(long userID, long labelID, string labelName)
        {
            try
            {
                await redis.RemoveNotesRedisCache(userID);
                return labelManagementRL.ChangeLabelName(userID, labelID, labelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Changes the name of the label.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="labelID">The label identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns></returns>
        public bool ChangeLabelName(long userID, long labelID, string labelName)
        {
            try
            {
                return ChangeLabelNameAsync(userID, labelID, labelName).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserLabelAsync(long userID, long labelID)
        {
            try
            {
                await redis.RemoveNotesRedisCache(userID);
                return labelManagementRL.DeleteUserLabel(userID, labelID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteUserLabel(long userID, long labelID)
        {
            try
            {
                return DeleteUserLabelAsync(userID, labelID).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<ResponseNoteModel>> GetLabelNotesAsync(long UserID, string labelName)
        {
            var cacheKey =  labelName + "LabelNotes:" + UserID.ToString();
            string serializedNotes;
            ICollection<ResponseNoteModel> Notes;
            try
            {
                var redisNoteCollection = await distributedCache.GetAsync(cacheKey);
                if (redisNoteCollection != null)
                {
                    serializedNotes = Encoding.UTF8.GetString(redisNoteCollection);
                    Notes = JsonConvert.DeserializeObject<List<ResponseNoteModel>>(serializedNotes);
                }
                else
                {
                    Notes = labelManagementRL.GetLabelNotes(UserID, labelName);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ICollection<ResponseNoteModel> GetLabelNotes(long UserID, string labelName) 
        {
            try
            {
                return GetLabelNotesAsync(UserID, labelName).Result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Gets the user labels.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public ICollection<ResponseLabel> GetUserLabels(long userID)
        {
            try
            {
                return GetUserLabelsAsync(userID).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ICollection<ResponseLabel>> GetUserLabelsAsync(long UserID)
        {
            var cacheKey = "UserLabels:" + UserID.ToString();
            string serializedNotes;
            ICollection<ResponseLabel> Labels;
            try
            {
                var redisNoteCollection = await distributedCache.GetAsync(cacheKey);
                if (redisNoteCollection != null)
                {
                    serializedNotes = Encoding.UTF8.GetString(redisNoteCollection);
                    Labels = JsonConvert.DeserializeObject<List<ResponseLabel>>(serializedNotes);
                }
                else
                {
                    Labels = labelManagementRL.GetUserLabels(UserID);
                    await redis.AddRedisCache(cacheKey, Labels);
                }
                return Labels;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
