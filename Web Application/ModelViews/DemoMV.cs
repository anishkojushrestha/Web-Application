namespace Web_Application.ModelViews
{
    public class DemoMV
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public int Mobile { get; set; }
        public int PanNo { get; set; }
        public string Remarks { get; set; }
        public string FeedBack { get; set; }

        public IFormFile Attechment { get; set; }
    }
}
