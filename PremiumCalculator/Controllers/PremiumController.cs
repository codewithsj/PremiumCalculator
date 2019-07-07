using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using PremiumCalculator.Models;
using System.Linq;

namespace PremiumCalculator.Controllers
{
    public class PremiumController : Controller
    {
        // GET: Premium
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get all the available Occupations
        /// </summary>
        /// <returns>Occupations list</returns>
        public JsonResult GetOccupations()
        {
            // TODO: Replace this with either WEB API call or Entity Framework 
            // to get data from the database
            WebClient webClient = new WebClient();
            string filePath = Server.MapPath(@"~\Content\sampleData\OccupationDetails.json");
            string jsonData = webClient.DownloadString(filePath);
            Occupation[] results = JsonConvert.DeserializeObject<Occupation[]>(jsonData);

            // Sort the list by name
            var orderedData = results.OrderBy(x => x.Name);

            // Returns the occupations list
            return Json(orderedData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calculate the montly premium
        /// </summary>
        /// <param name="premium">Premium object</param>
        /// <returns>Monthly premium</returns>
        public JsonResult CalculatePremium(Premium premium)
        {
            // Calculate the monthly premium
            double deathPremium = 0;

            if (premium.OccupationID != null)
            {
                // Get the occupation details
                Occupation occupationObj = GetOccupationDetails(premium.OccupationID.Value);

                // Get the rating details
                Rating ratingObj = GetRatingDetails(occupationObj.RatingID);

                // Get the factor
                double factor = ratingObj.Factor;

                // Calculate the monthly premium
                deathPremium = ((premium.DeathSumInsured * ratingObj.Factor * premium.Age) / 1000) * 12;

            }

            // Store the result to display on the UI
            string result = $"Based on the information provided, Your monthly premium is: {deathPremium}";

            // Return the calcualted monthly premium amount.
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region Private Methods

        /// <summary>
        /// Get the Rating details by ID 
        /// </summary>
        /// <param name="ID">Rating ID</param>
        /// <returns>Rating corresponding to the given ID</returns>
        private Rating GetRatingDetails(int ID)
        {
            // TODO: Replace this with either WEB API call or Entity Framework 
            // to get data from the database
            WebClient webClient = new WebClient();
            string filePath = Server.MapPath(@"~\Content\sampleData\RatingDetails.json");
            string jsonData = webClient.DownloadString(filePath);
            Rating[] results = JsonConvert.DeserializeObject<Rating[]>(jsonData);

            // Get the Rating details for the given ID
            var ratingData = results.FirstOrDefault(x => x.ID == ID);

            // Returns Rating Data
            return ratingData;
        }

        /// <summary>
        /// Get Occupation details by ID
        /// </summary>
        /// <param name="ID">Occupation ID</param>
        /// <returns>Occupation corresponding to the given ID</returns>
        private Occupation GetOccupationDetails(int ID)
        {
            // TODO: Replace this with either WEB API call or Entity Framework 
            // to get data from the database
            WebClient webClient = new WebClient();
            string filePath = Server.MapPath(@"~\Content\sampleData\OccupationDetails.json");
            string jsonData = webClient.DownloadString(filePath);
            Occupation[] results = JsonConvert.DeserializeObject<Occupation[]>(jsonData);

            // Get the Occupation details for the given ID
            Occupation occupationData = results.FirstOrDefault(x => x.ID == ID);

            // Returns Occupation Data
            return occupationData;
        }

        #endregion
    }
}