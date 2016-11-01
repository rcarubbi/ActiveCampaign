using System.Collections.Generic;

namespace ActiveCampaign
{
    public class SyncContactInput
    {
        public SyncContactInput()
        {
            CustomFields = new List<CustomField>();
        }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ListId { get; set; }

        public string Tags { get; set; }

        public List<CustomField> CustomFields { get; set; }
    }
}