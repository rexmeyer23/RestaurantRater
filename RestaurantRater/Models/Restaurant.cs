﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantRater.Models
{
    public enum TypeOfRestaurant { FastFood, Comfort, Steakhouse, Italian, Mexican, Asian, Indian }
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public TypeOfRestaurant RestaurantType { get; set; }

        [Required]
        public string Address { get; set; }


        public virtual List<Rating> Ratings { get; set; } = new List<Rating>();

        public double Rating
        {
            get
            {
                double totalAverageRating = 0;

                //add all ratings together
                foreach(var rating in Ratings)
                {
                    totalAverageRating += rating.AverageRating;
                }

                // get average from total
                return Ratings.Count > 0
                    ? Math.Round(totalAverageRating / Ratings.Count, 2) // If Ratings.Count > 0
                    : 0; // If Ratings.Count not > 0
            }
        }


        public bool IsRecommended
        {
            get
            {
                return Rating > 8;
            }
        }
        // Average Food Rating

        // Average Cleanliness Rating

        // Average Environment Rating
    }
}