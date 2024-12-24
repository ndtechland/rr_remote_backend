﻿namespace RR_Remote.Models.Entity
{
    public class Brand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}